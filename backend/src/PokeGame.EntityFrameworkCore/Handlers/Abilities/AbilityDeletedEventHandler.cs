using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Abilities.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers.Abilities;

internal class AbilityDeletedEventHandler : INotificationHandler<AbilityDeletedEvent>
{
  private readonly PokemonContext _context;

  public AbilityDeletedEventHandler(PokemonContext context)
  {
    _context = context;
  }

  public async Task Handle(AbilityDeletedEvent @event, CancellationToken cancellationToken)
  {
    AbilityEntity? ability = await _context.Abilities
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
    if (ability != null)
    {
      _context.Abilities.Remove(ability);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
