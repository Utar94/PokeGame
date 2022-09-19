using MediatR;
using PokeGame.Domain.Pokemon.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Pokemon
{
  internal class PokemonWoundedHandler : INotificationHandler<PokemonWounded>
  {
    private readonly SynchronizePokemon _synchronizePokemon;

    public PokemonWoundedHandler(SynchronizePokemon synchronizePokemon)
    {
      _synchronizePokemon = synchronizePokemon;
    }

    public async Task Handle(PokemonWounded notification, CancellationToken cancellationToken)
    {
      await _synchronizePokemon.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
