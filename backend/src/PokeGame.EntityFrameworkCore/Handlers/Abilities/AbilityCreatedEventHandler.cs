using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Abilities.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers.Abilities;

internal class AbilityCreatedEventHandler : INotificationHandler<AbilityCreatedEvent>
{
  private readonly PokemonContext _context;

  public AbilityCreatedEventHandler(PokemonContext context)
  {
    _context = context;
  }

  public async Task Handle(AbilityCreatedEvent @event, CancellationToken cancellationToken)
  {
    AbilityEntity? ability = await _context.Abilities.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
    if (ability == null)
    {
      ability = new(@event);

      _context.Abilities.Add(ability);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
