using MediatR;
using PokeGame.Domain.Pokemon.Events;

namespace PokeGame.ReadModel.Handlers.Pokemon
{
  internal class PokemonHealedHandler : INotificationHandler<PokemonHealed>
  {
    private readonly SynchronizePokemon _synchronizePokemon;

    public PokemonHealedHandler(SynchronizePokemon synchronizePokemon)
    {
      _synchronizePokemon = synchronizePokemon;
    }

    public async Task Handle(PokemonHealed notification, CancellationToken cancellationToken)
    {
      await _synchronizePokemon.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
