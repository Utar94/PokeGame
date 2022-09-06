using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Models;
using PokeGame.Application.Species;
using PokeGame.Application.Species.Models;
using PokeGame.Domain;

namespace PokeGame.Infrastructure.ReadModel.Queriers
{
  internal class SpeciesQuerier : ISpeciesQuerier
  {
    private readonly IMapper _mapper;
    private readonly DbSet<Entities.Species> _species;

    public SpeciesQuerier(IMapper mapper, ReadContext readContext)
    {
      _mapper = mapper;
      _species = readContext.Species;
    }

    public async Task<SpeciesModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Entities.Species? species = await _species.AsNoTracking()
        .Include(x => x.Abilities)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      return species == null ? null : _mapper.Map<SpeciesModel>(species);
    }

    public async Task<ListModel<SpeciesModel>> GetPagedAsync(string? search, PokemonType? type,
      SpeciesSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IQueryable<Entities.Species> query = _species.AsNoTracking();

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

      Entities.Species[] species = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = _mapper.Map<IEnumerable<SpeciesModel>>(species),
        Total = total
      };
    }
  }
}
