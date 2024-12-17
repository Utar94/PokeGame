using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Moves.Events;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure.Handlers;

internal class MoveEvents : INotificationHandler<MoveCreated>, INotificationHandler<MoveDeleted>, INotificationHandler<MoveUpdated>
{
  private readonly PokeGameContext _context;

  public MoveEvents(PokeGameContext context)
  {
    _context = context;
  }

  public async Task Handle(MoveCreated @event, CancellationToken cancellationToken)
  {
    MoveEntity? move = await _context.Moves.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (move == null)
    {
      move = new(@event);

      _context.Moves.Add(move);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(MoveDeleted @event, CancellationToken cancellationToken)
  {
    MoveEntity? move = await _context.Moves
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (move != null)
    {
      _context.Moves.Remove(move);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(MoveUpdated @event, CancellationToken cancellationToken)
  {
    MoveEntity move = await _context.Moves
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The move entity 'StreamId={@event.StreamId}' could not be found.");

    move.Update(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
