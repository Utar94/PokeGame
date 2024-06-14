using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Moves.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers.Moves;

internal class MoveCreatedEventHandler : INotificationHandler<MoveCreatedEvent>
{
  private readonly PokemonContext _context;

  public MoveCreatedEventHandler(PokemonContext context)
  {
    _context = context;
  }

  public async Task Handle(MoveCreatedEvent @event, CancellationToken cancellationToken)
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
