using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Moves.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Moves
{
  internal abstract class SynchronizeMove
  {
    protected SynchronizeMove(ReadContext readContext, IRepository<Move> repository)
    {
      ReadContext = readContext;
      Repository = repository;
    }

    protected ReadContext ReadContext { get; }
    protected IRepository<Move> Repository { get; }

    protected async Task SynchronizeAsync(Guid id, int version, CancellationToken cancellationToken)
    {
      Entities.Move? entity = await ReadContext.Moves
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      if (entity == null)
      {
        entity = new Entities.Move { Id = id };
        ReadContext.Moves.Add(entity);
      }
      else if (entity.Version >= version)
      {
        return;
      }

      Move move = await Repository.LoadAsync(id, version, cancellationToken)
        ?? throw new EntityNotFoundException<Move>(id);

      entity.Synchronize(move);

      await ReadContext.SaveChangesAsync(cancellationToken);
    }
  }

  internal class MoveCreatedHandler : SynchronizeMove, INotificationHandler<MoveCreated>
  {
    public MoveCreatedHandler(ReadContext readContext, IRepository<Move> repository)
      : base(readContext, repository)
    {
    }

    public async Task Handle(MoveCreated notification, CancellationToken cancellationToken)
    {
      await SynchronizeAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }

  internal class MoveUpdatedHandler : SynchronizeMove, INotificationHandler<MoveUpdated>
  {
    public MoveUpdatedHandler(ReadContext readContext, IRepository<Move> repository)
      : base(readContext, repository)
    {
    }

    public async Task Handle(MoveUpdated notification, CancellationToken cancellationToken)
    {
      await SynchronizeAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
