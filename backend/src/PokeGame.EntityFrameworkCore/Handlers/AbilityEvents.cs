using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Abilities;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers;

internal static class AbilityEvents
{
  public class AbilityCreatedEventHandler : INotificationHandler<Ability.CreatedEvent>
  {
    private readonly PokeGameContext _context;

    public AbilityCreatedEventHandler(PokeGameContext context)
    {
      _context = context;
    }

    public async Task Handle(Ability.CreatedEvent @event, CancellationToken cancellationToken)
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

  public class AbilityDeletedEventHandler : INotificationHandler<Ability.DeletedEvent>
  {
    private readonly PokeGameContext _context;

    public AbilityDeletedEventHandler(PokeGameContext context)
    {
      _context = context;
    }

    public async Task Handle(Ability.DeletedEvent @event, CancellationToken cancellationToken)
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

  public class AbilityUpdatedEventHandler : INotificationHandler<Ability.UpdatedEvent>
  {
    private readonly PokeGameContext _context;

    public AbilityUpdatedEventHandler(PokeGameContext context)
    {
      _context = context;
    }

    public async Task Handle(Ability.UpdatedEvent @event, CancellationToken cancellationToken)
    {
      AbilityEntity ability = await _context.Abilities
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The ability entity 'AggregateId={@event.AggregateId}' could not be found.");

      ability.Update(@event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
