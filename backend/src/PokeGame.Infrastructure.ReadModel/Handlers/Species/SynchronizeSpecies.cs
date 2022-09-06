using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Species.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Species
{
  internal abstract class SynchronizeSpecies
  {
    protected SynchronizeSpecies(ReadContext readContext, IRepository<Domain.Species.Species> repository)
    {
      ReadContext = readContext;
      Repository = repository;
    }

    protected ReadContext ReadContext { get; }
    protected IRepository<Domain.Species.Species> Repository { get; }

    protected async Task SynchronizeAsync(Guid id, int version, CancellationToken cancellationToken)
    {
      Entities.Species? entity = await ReadContext.Species
        .Include(x => x.Abilities)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      if (entity == null)
      {
        entity = new Entities.Species { Id = id };
        ReadContext.Species.Add(entity);
      }
      else if (entity.Version >= version)
      {
        return;
      }

      Domain.Species.Species species = await Repository.LoadAsync(id, version, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(id);

      entity.Synchronize(species);

      entity.Abilities.Clear();
      if (species.AbilityIds.Any())
      {
        entity.Abilities.AddRange(await ReadContext.Abilities
          .Where(x => species.AbilityIds.Contains(x.Id))
          .ToArrayAsync(cancellationToken));
      }

      await ReadContext.SaveChangesAsync(cancellationToken);
    }
  }

  internal class SpeciesCreatedHandler : SynchronizeSpecies, INotificationHandler<SpeciesCreated>
  {
    public SpeciesCreatedHandler(ReadContext readContext, IRepository<Domain.Species.Species> repository)
      : base(readContext, repository)
    {
    }

    public async Task Handle(SpeciesCreated notification, CancellationToken cancellationToken)
    {
      await SynchronizeAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }

  internal class SpeciesUpdatedHandler : SynchronizeSpecies, INotificationHandler<SpeciesUpdated>
  {
    public SpeciesUpdatedHandler(ReadContext readContext, IRepository<Domain.Species.Species> repository)
      : base(readContext, repository)
    {
    }

    public async Task Handle(SpeciesUpdated notification, CancellationToken cancellationToken)
    {
      await SynchronizeAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
