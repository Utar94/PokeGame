using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Moves;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers;

internal static class MoveEvents
{
  public class MoveCreatedEventHandler : INotificationHandler<Move.CreatedEvent>
  {
    private readonly PokeGameContext _context;

    public MoveCreatedEventHandler(PokeGameContext context)
    {
      _context = context;
    }

    public async Task Handle(Move.CreatedEvent @event, CancellationToken cancellationToken)
    {
      MoveEntity? move = await _context.Moves.AsNoTracking()
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
      if (move == null)
      {
        move = new(@event);

        _context.Moves.Add(move);

        await _context.SaveChangesAsync(cancellationToken);
      }
    }
  }

  public class MoveDeletedEventHandler : INotificationHandler<Move.DeletedEvent>
  {
    private readonly PokeGameContext _context;

    public MoveDeletedEventHandler(PokeGameContext context)
    {
      _context = context;
    }

    public async Task Handle(Move.DeletedEvent @event, CancellationToken cancellationToken)
    {
      MoveEntity? move = await _context.Moves
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
      if (move != null)
      {
        _context.Moves.Remove(move);

        await _context.SaveChangesAsync(cancellationToken);
      }
    }
  }

  public class MoveUpdatedEventHandler : INotificationHandler<Move.UpdatedEvent>
  {
    private readonly PokeGameContext _context;

    public MoveUpdatedEventHandler(PokeGameContext context)
    {
      _context = context;
    }

    public async Task Handle(Move.UpdatedEvent @event, CancellationToken cancellationToken)
    {
      MoveEntity move = await _context.Moves
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The move entity 'AggregateId={@event.AggregateId}' could not be found.");

      move.Update(@event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
