using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Models;
using PokeGame.Application.Moves;
using PokeGame.Application.Moves.Models;
using PokeGame.Domain;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Queriers
{
  internal class MoveQuerier : IMoveQuerier
  {
    private readonly IMappingService _mappingService;
    private readonly DbSet<MoveEntity> _moves;

    public MoveQuerier(IMappingService mappingService, ReadContext readContext)
    {
      _mappingService = mappingService;
      _moves = readContext.Moves;
    }

    public async Task<MoveModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      MoveEntity? move = await _moves.AsNoTracking()
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      return await _mappingService.MapAsync<MoveModel>(move, cancellationToken);
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
          MoveSort.UpdatedOn => desc ? query.OrderByDescending(x => x.UpdatedOn ?? x.CreatedOn) : query.OrderBy(x => x.UpdatedOn ?? x.CreatedOn),
          _ => throw new ArgumentException($"The move sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      MoveEntity[] moves = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = await _mappingService.MapAsync<MoveModel>(moves, cancellationToken),
        Total = total
      };
    }
  }
}
