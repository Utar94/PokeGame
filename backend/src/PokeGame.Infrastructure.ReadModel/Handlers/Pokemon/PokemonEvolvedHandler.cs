using MediatR;
using PokeGame.Domain.Pokemon.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Pokemon
{
  internal class PokemonEvolvedHandler : INotificationHandler<PokemonEvolved>
  {
    private readonly SynchronizePokemon _synchronizePokemon;

    public PokemonEvolvedHandler(SynchronizePokemon synchronizePokemon)
    {
      _synchronizePokemon = synchronizePokemon;
    }

    public async Task Handle(PokemonEvolved notification, CancellationToken cancellationToken)
    {
      await _synchronizePokemon.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
