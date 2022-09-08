using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Models;
using PokeGame.Application.Moves;
using PokeGame.Application.Moves.Models;
using PokeGame.Domain;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Queriers
{
  internal class MoveQuerier : IMoveQuerier
  {
    private readonly IMapper _mapper;
    private readonly DbSet<MoveEntity> _moves;

    public MoveQuerier(IMapper mapper, ReadContext readContext)
    {
      _mapper = mapper;
      _moves = readContext.Moves;
    }

    public async Task<MoveModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      MoveEntity? move = await _moves.AsNoTracking()
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      return move == null ? null : _mapper.Map<MoveModel>(move);
    }

    public async Task<ListModel<MoveModel>> GetPagedAsync(string? search, PokemonType? type,
      MoveSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IQueryable<MoveEntity> query = _moves.AsNoTracking();

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
      if (type.HasValue)
      {
        query = query.Where(x => x.Type == type.Value);
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          MoveSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          MoveSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The move sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      MoveEntity[] moves = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = _mapper.Map<IEnumerable<MoveModel>>(moves),
        Total = total
      };
    }
  }
}
