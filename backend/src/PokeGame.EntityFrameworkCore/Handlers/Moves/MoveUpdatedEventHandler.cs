using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Moves.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers.Moves;

internal class MoveUpdatedEventHandler : INotificationHandler<MoveUpdatedEvent>
{
  private readonly PokemonContext _context;

  public MoveUpdatedEventHandler(PokemonContext context)
  {
    _context = context;
  }

  public async Task Handle(MoveUpdatedEvent @event, CancellationToken cancellationToken)
  {
    MoveEntity? move = await _context.Moves
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The move entity 'AggregateId={@event.AggregateId}' could not be found.");

    move.Update(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
