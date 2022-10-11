using MediatR;
using PokeGame.Domain.Pokemon.Events;

namespace PokeGame.ReadModel.Handlers.Pokemon
{
  internal class PokemonMovedHandler : INotificationHandler<PokemonMoved>
  {
    private readonly SynchronizePokemon _synchronizePokemon;

    public PokemonMovedHandler(SynchronizePokemon synchronizePokemon)
    {
      _synchronizePokemon = synchronizePokemon;
    }

    public async Task Handle(PokemonMoved notification, CancellationToken cancellationToken)
    {
      await _synchronizePokemon.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
