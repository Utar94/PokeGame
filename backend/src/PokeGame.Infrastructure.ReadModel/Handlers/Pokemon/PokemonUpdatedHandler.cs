using MediatR;
using PokeGame.Domain.Pokemon.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Pokemon
{
  internal class PokemonUpdatedHandler : INotificationHandler<PokemonUpdated>
  {
    private readonly SynchronizePokemon _synchronizePokemon;

    public PokemonUpdatedHandler(SynchronizePokemon synchronizePokemon)
    {
      _synchronizePokemon = synchronizePokemon;
    }

    public async Task Handle(PokemonUpdated notification, CancellationToken cancellationToken)
    {
      await _synchronizePokemon.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
