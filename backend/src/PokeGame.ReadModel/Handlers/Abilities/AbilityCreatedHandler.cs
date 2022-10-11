using MediatR;
using PokeGame.Domain.Abilities.Events;

namespace PokeGame.ReadModel.Handlers.Abilities
{
  internal class AbilityCreatedHandler : INotificationHandler<AbilityCreated>
  {
    private readonly SynchronizeAbility _synchronizeAbility;

    public AbilityCreatedHandler(SynchronizeAbility synchronizeAbility)
    {
      _synchronizeAbility = synchronizeAbility;
    }

    public async Task Handle(AbilityCreated notification, CancellationToken cancellationToken)
    {
      await _synchronizeAbility.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
