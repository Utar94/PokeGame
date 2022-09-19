using MediatR;
using PokeGame.Domain.Pokemon.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Pokemon
{
  internal class PokemonUsedMoveHandler : INotificationHandler<PokemonUsedMove>
  {
    private readonly SynchronizePokemon _synchronizePokemon;

    public PokemonUsedMoveHandler(SynchronizePokemon synchronizePokemon)
    {
      _synchronizePokemon = synchronizePokemon;
    }

    public async Task Handle(PokemonUsedMove notification, CancellationToken cancellationToken)
    {
      await _synchronizePokemon.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
