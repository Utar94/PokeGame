using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Abilities.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers.Abilities;

internal class AbilityUpdatedEventHandler : INotificationHandler<AbilityUpdatedEvent>
{
  private readonly PokemonContext _context;

  public AbilityUpdatedEventHandler(PokemonContext context)
  {
    _context = context;
  }

  public async Task Handle(AbilityUpdatedEvent @event, CancellationToken cancellationToken)
  {
    AbilityEntity? ability = await _context.Abilities
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The ability entity 'AggregateId={@event.AggregateId}' could not be found.");

    ability.Update(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
