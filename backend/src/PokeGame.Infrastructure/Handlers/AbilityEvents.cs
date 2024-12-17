using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Abilities.Events;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure.Handlers;

internal class AbilityEvents : INotificationHandler<AbilityCreated>, INotificationHandler<AbilityDeleted>, INotificationHandler<AbilityUpdated>
{
  private readonly PokeGameContext _context;

  public AbilityEvents(PokeGameContext context)
  {
    _context = context;
  }

  public async Task Handle(AbilityCreated @event, CancellationToken cancellationToken)
  {
    AbilityEntity? ability = await _context.Abilities.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (ability == null)
    {
      ability = new(@event);

      _context.Abilities.Add(ability);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(AbilityDeleted @event, CancellationToken cancellationToken)
  {
    AbilityEntity? ability = await _context.Abilities
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (ability != null)
    {
      _context.Abilities.Remove(ability);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(AbilityUpdated @event, CancellationToken cancellationToken)
  {
    AbilityEntity ability = await _context.Abilities
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The ability entity 'StreamId={@event.StreamId}' could not be found.");

    ability.Update(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
