using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Items.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers.Items;

internal class ItemUpdatedEventHandler : INotificationHandler<ItemUpdatedEvent>
{
  private readonly PokemonContext _context;

  public ItemUpdatedEventHandler(PokemonContext context)
  {
    _context = context;
  }

  public async Task Handle(ItemUpdatedEvent @event, CancellationToken cancellationToken)
  {
    ItemEntity? item = await _context.Items
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The item entity 'AggregateId={@event.AggregateId}' could not be found.");

    item.Update(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
