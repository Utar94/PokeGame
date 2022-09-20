using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Moves;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Moves
{
  internal class SynchronizeMove
  {
    private readonly ReadContext _readContext;
    private readonly IRepository _repository;

    public SynchronizeMove(ReadContext readContext, IRepository repository)
    {
      _readContext = readContext;
      _repository = repository;
    }

    public async Task<MoveEntity?> ExecuteAsync(Guid id, int? version = null, CancellationToken cancellationToken = default)
    {
      MoveEntity? entity = await _readContext.Moves
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      if (entity != null && version.HasValue && entity.Version >= version.Value)
      {
        return entity;
      }

      Move? move = await _repository.LoadAsync<Move>(id, version, cancellationToken);
      if (move != null)
      {
        if (entity == null)
        {
          entity = new MoveEntity { Id = id };
          _readContext.Moves.Add(entity);
        }

        entity.Synchronize(move);

        await _readContext.SaveChangesAsync(cancellationToken);
      }

      return entity;
    }

    public async Task<IEnumerable<MoveEntity>> ExecuteAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
      Dictionary<Guid, MoveEntity> entities = await _readContext.Moves
        .Where(x => ids.Contains(x.Id))
        .ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

      IEnumerable<Move> moves = await _repository.LoadAsync<Move>(ids, cancellationToken);
      foreach (Move move in moves)
      {
        if (entities.TryGetValue(move.Id, out MoveEntity? entity))
        {
          if (entity.Version >= move.Version)
          {
            continue;
          }
        }
        else
        {
          entity = new MoveEntity { Id = move.Id };
          entities[entity.Id] = entity;
          _readContext.Moves.Add(entity);
        }

        entity.Synchronize(move);
      }

      await _readContext.SaveChangesAsync(cancellationToken);

      return entities.Values;
    }
  }
}
