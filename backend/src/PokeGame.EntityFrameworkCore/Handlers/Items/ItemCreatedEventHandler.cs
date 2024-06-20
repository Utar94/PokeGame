using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Items.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers.Items;

internal class ItemCreatedEventHandler : INotificationHandler<ItemCreatedEvent>
{
  private readonly PokemonContext _context;

  public ItemCreatedEventHandler(PokemonContext context)
  {
    _context = context;
  }

  public async Task Handle(ItemCreatedEvent @event, CancellationToken cancellationToken)
  {
    ItemEntity? item = await _context.Items.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
    if (item == null)
    {
      item = new(@event);

      _context.Items.Add(item);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
