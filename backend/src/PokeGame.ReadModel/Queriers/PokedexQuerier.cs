using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Models;
using PokeGame.Application.Pokedex;
using PokeGame.Application.Pokedex.Models;
using PokeGame.Domain;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Queriers
{
  internal class PokedexQuerier : IPokedexQuerier
  {
    private readonly IMappingService _mappingService;
    private readonly DbSet<PokedexEntity> _pokedex;

    public PokedexQuerier(IMappingService mappingService, ReadContext readContext)
    {
      _mappingService = mappingService;
      _pokedex = readContext.Pokedex;
    }

    public async Task<PokedexModel?> GetAsync(Guid trainerId, Guid speciesId, CancellationToken cancellationToken)
    {
      PokedexEntity? pokedex = await _pokedex.AsNoTracking()
        .Include(x => x.Species)
        .Include(x => x.Trainer)
        .SingleOrDefaultAsync(x => x.Trainer!.Id == trainerId && x.Species!.Id == speciesId, cancellationToken);

      return await _mappingService.MapAsync<PokedexModel>(pokedex, cancellationToken);
    }

    public async Task<ListModel<PokedexModel>> GetPagedAsync(Guid trainerId, bool? hasCaught, Region? region, string? search, PokemonType? type,
      PokedexSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IQueryable<PokedexEntity> query = _pokedex.AsNoTracking()
        .Include(x => x.Species).ThenInclude(x => x!.RegionalSpecies)
        .Where(x => x.Trainer!.Id == trainerId);

      if (hasCaught.HasValue)
      {
        query = query.Where(x => x.HasCaught == hasCaught.Value);
      }
      if (region.HasValue)
      {
        query = query.Where(x => x.Species!.RegionalSpecies.Any(y => y.Region == region.Value));
      }
      if (search != null)
      {
        foreach (string term in search.Split())
        {
          if (!string.IsNullOrEmpty(term))
          {
            string pattern = $"%{term}%";

            query = query.Where(x => EF.Functions.ILike(x.Species!.Name, pattern)
              || (x.Species!.Category != null && EF.Functions.ILike(x.Species.Category, pattern)));
          }
        }
      }
      if (type.HasValue)
      {
        query = query.Where(x => x.Species!.PrimaryType == type.Value || x.Species.SecondaryType == type.Value);
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          PokedexSort.Height => desc ? query.OrderByDescending(x => x.Species!.Height) : query.OrderBy(x => x.Species!.Height),
          PokedexSort.Name => desc ? query.OrderByDescending(x => x.Species!.Name) : query.OrderBy(x => x.Species!.Name),
          PokedexSort.Number => desc ? query.OrderByDescending(x => x.Species!.Number) : query.OrderBy(x => x.Species!.Number),
          PokedexSort.UpdatedOn => desc ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn),
          PokedexSort.Weight => desc ? query.OrderByDescending(x => x.Species!.Weight) : query.OrderBy(x => x.Species!.Weight),
          _ => throw new ArgumentException($"The Pokédex sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      PokedexEntity[] pokedex = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = (await _mappingService.MapAsync<IEnumerable<PokedexModel>>(pokedex, cancellationToken))!,
        Total = total
      };
    }
  }
}
