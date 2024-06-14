using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Moves.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers.Moves;

internal class MoveDeletedEventHandler : INotificationHandler<MoveDeletedEvent>
{
  private readonly PokemonContext _context;

  public MoveDeletedEventHandler(PokemonContext context)
  {
    _context = context;
  }

  public async Task Handle(MoveDeletedEvent @event, CancellationToken cancellationToken)
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
