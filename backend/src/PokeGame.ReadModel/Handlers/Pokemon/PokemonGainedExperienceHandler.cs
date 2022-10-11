using MediatR;
using PokeGame.Domain.Pokemon.Events;

namespace PokeGame.ReadModel.Handlers.Pokemon
{
  internal class PokemonGainedExperienceHandler : INotificationHandler<PokemonGainedExperience>
  {
    private readonly SynchronizePokemon _synchronizePokemon;

    public PokemonGainedExperienceHandler(SynchronizePokemon synchronizePokemon)
    {
      _synchronizePokemon = synchronizePokemon;
    }

    public async Task Handle(PokemonGainedExperience notification, CancellationToken cancellationToken)
    {
      await _synchronizePokemon.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
