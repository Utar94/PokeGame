using MediatR;
using PokeGame.Domain.Pokemon.Events;

namespace PokeGame.ReadModel.Handlers.Pokemon
{
  internal class PokemonCaughtHandler : INotificationHandler<PokemonCaught>
  {
    private readonly SynchronizePokemon _synchronizePokemon;

    public PokemonCaughtHandler(SynchronizePokemon synchronizePokemon)
    {
      _synchronizePokemon = synchronizePokemon;
    }

    public async Task Handle(PokemonCaught notification, CancellationToken cancellationToken)
    {
      await _synchronizePokemon.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
