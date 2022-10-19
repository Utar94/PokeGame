using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Species.Events;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Handlers.Species
{
  internal class SpeciesEvolutionRemovedHandler : INotificationHandler<SpeciesEvolutionRemoved>
  {
    private readonly ReadContext _readContext;
    private readonly SynchronizeSpecies _synchronizeSpecies;

    public SpeciesEvolutionRemovedHandler(ReadContext readContext, SynchronizeSpecies synchronizeSpecies)
    {
      _readContext = readContext;
      _synchronizeSpecies = synchronizeSpecies;
    }

    public async Task Handle(SpeciesEvolutionRemoved notification, CancellationToken cancellationToken)
    {
      SpeciesEntity? evolvingSpecies = await _synchronizeSpecies
        .ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
      if (evolvingSpecies == null)
      {
        return;
      }

      SpeciesEntity? evolvedSpecies = await _synchronizeSpecies
        .ExecuteAsync(notification.SpeciesId, version: null, cancellationToken);
      if (evolvedSpecies == null)
      {
        return;
      }

      EvolutionEntity? entity = await _readContext.Evolutions
        .SingleOrDefaultAsync(x => x.EvolvingSpeciesId == evolvingSpecies.Sid && x.EvolvedSpeciesId == evolvedSpecies.Sid, cancellationToken);
      if (entity != null)
      {
        _readContext.Evolutions.Remove(entity);
        await _readContext.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
