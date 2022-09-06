using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Abilities;
using PokeGame.Domain.Abilities.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Abilities
{
  internal abstract class SynchronizeAbility
  {
    protected SynchronizeAbility(ReadContext readContext, IRepository<Ability> repository)
    {
      ReadContext = readContext;
      Repository = repository;
    }

    protected ReadContext ReadContext { get; }
    protected IRepository<Ability> Repository { get; }

    protected async Task SynchronizeAsync(Guid id, int version, CancellationToken cancellationToken)
    {
      Entities.Ability? entity = await ReadContext.Abilities
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      if (entity == null)
      {
        entity = new Entities.Ability { Id = id };
        ReadContext.Abilities.Add(entity);
      }
      else if (entity.Version >= version)
      {
        return;
      }

      Ability ability = await Repository.LoadAsync(id, version, cancellationToken)
        ?? throw new EntityNotFoundException<Ability>(id);

      entity.Synchronize(ability);

      await ReadContext.SaveChangesAsync(cancellationToken);
    }
  }

  internal class AbilityCreatedHandler : SynchronizeAbility, INotificationHandler<AbilityCreated>
  {
    public AbilityCreatedHandler(ReadContext readContext, IRepository<Ability> repository)
      : base(readContext, repository)
    {
    }

    public async Task Handle(AbilityCreated notification, CancellationToken cancellationToken)
    {
      await SynchronizeAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }

  internal class AbilityUpdatedHandler : SynchronizeAbility, INotificationHandler<AbilityUpdated>
  {
    public AbilityUpdatedHandler(ReadContext readContext, IRepository<Ability> repository)
      : base(readContext, repository)
    {
    }

    public async Task Handle(AbilityUpdated notification, CancellationToken cancellationToken)
    {
      await SynchronizeAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
