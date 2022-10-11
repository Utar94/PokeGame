using MediatR;
using PokeGame.Domain.Pokemon.Events;

namespace PokeGame.ReadModel.Handlers.Pokemon
{
  internal class PokemonCreatedHandler : INotificationHandler<PokemonCreated>
  {
    private readonly SynchronizePokemon _synchronizePokemon;

    public PokemonCreatedHandler(SynchronizePokemon synchronizePokemon)
    {
      _synchronizePokemon = synchronizePokemon;
    }

    public async Task Handle(PokemonCreated notification, CancellationToken cancellationToken)
    {
      await _synchronizePokemon.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
