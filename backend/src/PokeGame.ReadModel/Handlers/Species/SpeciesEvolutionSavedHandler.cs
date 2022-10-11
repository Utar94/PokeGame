using MediatR;
using PokeGame.Application;
using PokeGame.Domain.Species;
using PokeGame.Domain.Species.Events;
using PokeGame.ReadModel.Entities;
using PokeGame.ReadModel.Handlers.Items;
using PokeGame.ReadModel.Handlers.Moves;

namespace PokeGame.ReadModel.Handlers.Species
{
  internal class SpeciesEvolutionSavedHandler : INotificationHandler<SpeciesEvolutionSaved>
  {
    private readonly ReadContext _readContext;
    private readonly IRepository _repository;
    private readonly SynchronizeItem _synchronizeItem;
    private readonly SynchronizeMove _synchronizeMove;
    private readonly SynchronizeSpecies _synchronizeSpecies;

    public SpeciesEvolutionSavedHandler(
      ReadContext readContext,
      IRepository repository,
      SynchronizeItem synchronizeItem,
      SynchronizeMove synchronizeMove,
      SynchronizeSpecies synchronizeSpecies
    )
    {
      _readContext = readContext;
      _repository = repository;
      _synchronizeItem = synchronizeItem;
      _synchronizeMove = synchronizeMove;
      _synchronizeSpecies = synchronizeSpecies;
    }

    public async Task Handle(SpeciesEvolutionSaved notification, CancellationToken cancellationToken)
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

      Domain.Species.Species? species = await _repository
        .LoadAsync<Domain.Species.Species>(notification.AggregateId, notification.Version, cancellationToken);
      if (species == null)
      {
        return;
      }

      if (species.Evolutions.TryGetValue(notification.SpeciesId, out Evolution? evolution))
      {
        ItemEntity? item = null;
        if (evolution.ItemId.HasValue)
        {
          item = await _synchronizeItem.ExecuteAsync(evolution.ItemId.Value, version: null, cancellationToken);
          if (item == null)
          {
            return;
          }
        }

        MoveEntity? move = null;
        if (evolution.MoveId.HasValue)
        {
          move = await _synchronizeMove.ExecuteAsync(evolution.MoveId.Value, version: null, cancellationToken);
          if (move == null)
          {
            return;
          }
        }

        EvolutionEntity? entity = evolvingSpecies.Evolutions
          .SingleOrDefault(x => x.EvolvedSpeciesId == evolvedSpecies.Sid)
          ?? evolvingSpecies.Add(evolvedSpecies);

        entity.Synchronize(evolution, item, move);

        await _readContext.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
