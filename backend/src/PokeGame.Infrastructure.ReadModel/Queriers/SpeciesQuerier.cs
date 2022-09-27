using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Models;
using PokeGame.Application.Species;
using PokeGame.Application.Species.Models;
using PokeGame.Domain;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Queriers
{
  internal class SpeciesQuerier : ISpeciesQuerier
  {
    private readonly DbSet<EvolutionEntity> _evolutions;
    private readonly IMapper _mapper;
    private readonly DbSet<RegionalSpeciesEntity> _regionalSpecies;
    private readonly DbSet<SpeciesEntity> _species;

    public SpeciesQuerier(IMapper mapper, ReadContext readContext)
    {
      _evolutions = readContext.Evolutions;
      _mapper = mapper;
      _regionalSpecies = readContext.RegionalSpecies;
      _species = readContext.Species;
    }

    public async Task<SpeciesModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      SpeciesEntity? species = await _species.AsNoTracking()
        .Include(x => x.RegionalSpecies)
        .Include(x => x.SpeciesAbilities).ThenInclude(x => x.Ability)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      return species == null ? null : _mapper.Map<SpeciesModel>(species);
    }

    public async Task<SpeciesModel?> GetAsync(int number, CancellationToken cancellationToken)
    {
      SpeciesEntity? species = await _species.AsNoTracking()
        .Include(x => x.RegionalSpecies)
        .Include(x => x.SpeciesAbilities).ThenInclude(x => x.Ability)
        .SingleOrDefaultAsync(x => x.Number == number, cancellationToken);

      return species == null ? null : _mapper.Map<SpeciesModel>(species);
    }

    public async Task<EvolutionModel?> GetEvolutionAsync(Guid id, Guid speciesId, CancellationToken cancellationToken)
    {
      EvolutionEntity? evolution = await _evolutions.AsNoTracking()
        .Include(x => x.EvolvingSpecies)
        .Include(x => x.EvolvedSpecies)
        .Include(x => x.Item)
        .Include(x => x.Move)
        .SingleOrDefaultAsync(x => x.EvolvingSpecies!.Id == id && x.EvolvedSpecies!.Id == speciesId, cancellationToken);

      return evolution == null ? null : _mapper.Map<EvolutionModel>(evolution);
    }

    public async Task<IEnumerable<EvolutionModel>?> GetEvolutionsAsync(Guid id, CancellationToken cancellationToken)
    {
      SpeciesEntity? species = await _species.AsNoTracking()
        .Include(x => x.Evolutions).ThenInclude(x => x.EvolvedSpecies).ThenInclude(x => x!.SpeciesAbilities).ThenInclude(x => x.Ability)
        .Include(x => x.Evolutions).ThenInclude(x => x.Item)
        .Include(x => x.Evolutions).ThenInclude(x => x.Move)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      return species == null ? null : _mapper.Map<IEnumerable<EvolutionModel>>(species.Evolutions);
    }

    public async Task<ListModel<SpeciesModel>> GetPagedAsync(Region? region, string? search, PokemonType? type,
      SpeciesSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IQueryable<SpeciesEntity> query = _species.AsNoTracking()
        .Include(x => x.RegionalSpecies);

      if (region.HasValue)
      {
        query = query.Where(x => x.RegionalSpecies.Any(y => y.Region == region.Value));
      }
      if (search != null)
      {
        foreach (string term in search.Split())
        {
          if (!string.IsNullOrEmpty(term))
          {
            string pattern = $"%{term}%";

            query = query.Where(x => x.Number.ToString() == term
              || EF.Functions.ILike(x.Name, pattern)
              || (x.Category != null && EF.Functions.ILike(x.Category, pattern)));
          }
        }
      }
      if (type.HasValue)
      {
        query = query.Where(x => x.PrimaryType == type.Value || x.SecondaryType == type.Value);
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          SpeciesSort.Category => desc ? query.OrderByDescending(x => x.Category) : query.OrderBy(x => x.Category),
          SpeciesSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          SpeciesSort.Number => desc ? query.OrderByDescending(x => x.Number) : query.OrderBy(x => x.Number),
          SpeciesSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The species sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      SpeciesEntity[] species = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = _mapper.Map<IEnumerable<SpeciesModel>>(species),
        Total = total
      };
    }

    public async Task<Dictionary<Region, HashSet<int>>> GetRegionalNumbersAsync(IEnumerable<Region>? regions, CancellationToken cancellationToken)
    {
      IQueryable<RegionalSpeciesEntity> query = _regionalSpecies.AsNoTracking();

      if (regions != null)
      {
        query = query.Where(x => regions.Contains(x.Region));
      }

      RegionalSpeciesEntity[] regionalSpecies = await query.ToArrayAsync(cancellationToken);

      return regionalSpecies.GroupBy(x => x.Region).ToDictionary(x => x.Key, x => x.Select(y => y.Number).ToHashSet());
    }
  }
}
