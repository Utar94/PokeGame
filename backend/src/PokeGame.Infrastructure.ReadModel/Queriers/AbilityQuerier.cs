using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Abilities;
using PokeGame.Application.Abilities.Models;
using PokeGame.Application.Models;

namespace PokeGame.Infrastructure.ReadModel.Queriers
{
  internal class AbilityQuerier : IAbilityQuerier
  {
    private readonly DbSet<Entities.Ability> _abilities;
    private readonly IMapper _mapper;

    public AbilityQuerier(IMapper mapper, ReadContext readContext)
    {
      _abilities = readContext.Abilities;
      _mapper = mapper;
    }

    public async Task<AbilityModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Entities.Ability? ability = await _abilities.AsNoTracking()
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      return ability == null ? null : _mapper.Map<AbilityModel>(ability);
    }

    public async Task<ListModel<AbilityModel>> GetPagedAsync(string? search,
      AbilitySort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IQueryable<Entities.Ability> query = _abilities.AsNoTracking();

      if (search != null)
      {
        foreach (string term in search.Split())
        {
          if (!string.IsNullOrEmpty(term))
          {
            string pattern = $"%{term}%";

            query = query.Where(x => EF.Functions.ILike(x.Name, pattern));
          }
        }
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          AbilitySort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          AbilitySort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The ability sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      Entities.Ability[] abilities = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = _mapper.Map<IEnumerable<AbilityModel>>(abilities),
        Total = total
      };
    }
  }
}
