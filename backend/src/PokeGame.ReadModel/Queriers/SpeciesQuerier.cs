﻿using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Models;
using PokeGame.Application.Species;
using PokeGame.Application.Species.Models;
using PokeGame.Domain;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Queriers
{
  internal class SpeciesQuerier : ISpeciesQuerier
  {
    private readonly DbSet<EvolutionEntity> _evolutions;
    private readonly IMappingService _mappingService;
    private readonly DbSet<SpeciesEntity> _species;

    public SpeciesQuerier(IMappingService mappingService, ReadContext readContext)
    {
      _evolutions = readContext.Evolutions;
      _mappingService = mappingService;
      _species = readContext.Species;
    }

    public async Task<SpeciesModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      SpeciesEntity? species = await _species.AsNoTracking()
        .Include(x => x.RegionalSpecies).ThenInclude(x => x.Region)
        .Include(x => x.SpeciesAbilities).ThenInclude(x => x.Ability)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      return await _mappingService.MapAsync<SpeciesModel>(species, cancellationToken);
    }

    public async Task<SpeciesModel?> GetAsync(int number, CancellationToken cancellationToken)
    {
      SpeciesEntity? species = await _species.AsNoTracking()
        .Include(x => x.RegionalSpecies).ThenInclude(x => x.Region)
        .Include(x => x.SpeciesAbilities).ThenInclude(x => x.Ability)
        .SingleOrDefaultAsync(x => x.Number == number, cancellationToken);

      return await _mappingService.MapAsync<SpeciesModel>(species, cancellationToken);
    }

    public async Task<SpeciesModel?> GetAsync(Guid regionId, int number, CancellationToken cancellationToken)
    {
      SpeciesEntity? species = await _species.AsNoTracking()
        .Include(x => x.RegionalSpecies).ThenInclude(x => x.Region)
        .Include(x => x.SpeciesAbilities).ThenInclude(x => x.Ability)
        .SingleOrDefaultAsync(x => x.RegionalSpecies.Any(y => y.Region!.Id == regionId && y.Number == number), cancellationToken);

      return await _mappingService.MapAsync<SpeciesModel>(species, cancellationToken);
    }

    public async Task<EvolutionModel?> GetEvolutionAsync(Guid id, Guid speciesId, CancellationToken cancellationToken)
    {
      EvolutionEntity? evolution = await _evolutions.AsNoTracking()
        .Include(x => x.EvolvingSpecies)
        .Include(x => x.EvolvedSpecies)
        .Include(x => x.Item)
        .Include(x => x.Move)
        .Include(x => x.Region)
        .SingleOrDefaultAsync(x => x.EvolvingSpecies!.Id == id && x.EvolvedSpecies!.Id == speciesId, cancellationToken);

      return await _mappingService.MapAsync<EvolutionModel>(evolution, cancellationToken);
    }

    public async Task<IEnumerable<EvolutionModel>?> GetEvolutionsAsync(Guid id, CancellationToken cancellationToken)
    {
      SpeciesEntity? species = await _species.AsNoTracking()
        .Include(x => x.Evolutions).ThenInclude(x => x.EvolvedSpecies).ThenInclude(x => x!.SpeciesAbilities).ThenInclude(x => x.Ability)
        .Include(x => x.Evolutions).ThenInclude(x => x.Item)
        .Include(x => x.Evolutions).ThenInclude(x => x.Move)
        .Include(x => x.Evolutions).ThenInclude(x => x.Region)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      return species == null ? null : await _mappingService.MapAsync<IEnumerable<EvolutionModel>>(species.Evolutions, cancellationToken);
    }

    public async Task<ListModel<SpeciesModel>> GetPagedAsync(Guid? regionId, string? search, PokemonType? type,
      SpeciesSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IQueryable<SpeciesEntity> query = _species.AsNoTracking()
        .Include(x => x.RegionalSpecies).ThenInclude(x => x.Region);

      if (regionId.HasValue)
      {
        query = query.Where(x => x.RegionalSpecies.Any(y => y.Region!.Id == regionId.Value));
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
          SpeciesSort.UpdatedOn => desc ? query.OrderByDescending(x => x.UpdatedOn ?? x.CreatedOn) : query.OrderBy(x => x.UpdatedOn ?? x.CreatedOn),
          _ => throw new ArgumentException($"The species sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      SpeciesEntity[] species = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = await _mappingService.MapAsync<SpeciesModel>(species, cancellationToken),
        Total = total
      };
    }
  }
}
