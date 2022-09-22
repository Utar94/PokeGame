using MediatR;
using PokeGame.Domain.Pokemon.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Pokemon
{
  internal class PokemonHeldItemHandler : INotificationHandler<PokemonHeldItem>
  {
    private readonly SynchronizePokemon _synchronizePokemon;

    public PokemonHeldItemHandler(SynchronizePokemon synchronizePokemon)
    {
      _synchronizePokemon = synchronizePokemon;
    }

    public async Task Handle(PokemonHeldItem notification, CancellationToken cancellationToken)
    {
      await _synchronizePokemon.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
