using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Abilities.Events;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Abilities
{
  internal class AbilityDeletedHandler : INotificationHandler<AbilityDeleted>
  {
    private readonly ReadContext _readContext;

    public AbilityDeletedHandler(ReadContext readContext)
    {
      _readContext = readContext;
    }

    public async Task Handle(AbilityDeleted notification, CancellationToken cancellationToken)
    {
      Ability? ability = await _readContext.Abilities
        .SingleOrDefaultAsync(x => x.Id == notification.AggregateId, cancellationToken);

      if (ability != null)
      {
        _readContext.Abilities.Remove(ability);
        await _readContext.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
