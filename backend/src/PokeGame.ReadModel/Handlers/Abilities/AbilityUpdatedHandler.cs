using MediatR;
using PokeGame.Domain.Abilities.Events;

namespace PokeGame.ReadModel.Handlers.Abilities
{
  internal class AbilityUpdatedHandler : INotificationHandler<AbilityUpdated>
  {
    private readonly SynchronizeAbility _synchronizeAbility;

    public AbilityUpdatedHandler(SynchronizeAbility synchronizeAbility)
    {
      _synchronizeAbility = synchronizeAbility;
    }

    public async Task Handle(AbilityUpdated notification, CancellationToken cancellationToken)
    {
      await _synchronizeAbility.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
