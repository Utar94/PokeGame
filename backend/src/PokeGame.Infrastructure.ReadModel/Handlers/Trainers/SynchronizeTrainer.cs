using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Trainers;
using PokeGame.Domain.Trainers.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Trainers
{
  internal abstract class SynchronizeTrainer
  {
    protected SynchronizeTrainer(ReadContext readContext, IRepository<Trainer> repository)
    {
      ReadContext = readContext;
      Repository = repository;
    }

    protected ReadContext ReadContext { get; }
    protected IRepository<Trainer> Repository { get; }

    protected async Task SynchronizeAsync(Guid id, int version, CancellationToken cancellationToken)
    {
      Entities.Trainer? entity = await ReadContext.Trainers
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      if (entity == null)
      {
        entity = new Entities.Trainer { Id = id };
        ReadContext.Trainers.Add(entity);
      }
      else if (entity.Version >= version)
      {
        return;
      }

      Trainer trainer = await Repository.LoadAsync(id, version, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(id);

      entity.Synchronize(trainer);

      await ReadContext.SaveChangesAsync(cancellationToken);
    }
  }

  internal class TrainerCreatedHandler : SynchronizeTrainer, INotificationHandler<TrainerCreated>
  {
    public TrainerCreatedHandler(ReadContext readContext, IRepository<Trainer> repository)
      : base(readContext, repository)
    {
    }

    public async Task Handle(TrainerCreated notification, CancellationToken cancellationToken)
    {
      await SynchronizeAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }

  internal class TrainerUpdatedHandler : SynchronizeTrainer, INotificationHandler<TrainerUpdated>
  {
    public TrainerUpdatedHandler(ReadContext readContext, IRepository<Trainer> repository)
      : base(readContext, repository)
    {
    }

    public async Task Handle(TrainerUpdated notification, CancellationToken cancellationToken)
    {
      await SynchronizeAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
