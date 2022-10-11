using MediatR;
using PokeGame.Domain.Pokemon.Events;

namespace PokeGame.ReadModel.Handlers.Pokemon
{
  internal class UpdatedPokemonConditionHandler : INotificationHandler<UpdatedPokemonCondition>
  {
    private readonly SynchronizePokemon _synchronizePokemon;

    public UpdatedPokemonConditionHandler(SynchronizePokemon synchronizePokemon)
    {
      _synchronizePokemon = synchronizePokemon;
    }

    public async Task Handle(UpdatedPokemonCondition notification, CancellationToken cancellationToken)
    {
      await _synchronizePokemon.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
