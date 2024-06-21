using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Items.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers.Items;

internal class ItemDeletedEventHandler : INotificationHandler<ItemDeletedEvent>
{
  private readonly PokemonContext _context;

  public ItemDeletedEventHandler(PokemonContext context)
  {
    _context = context;
  }

  public async Task Handle(ItemDeletedEvent @event, CancellationToken cancellationToken)
  {
    ItemEntity? item = await _context.Items
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
    if (item != null)
    {
      _context.Items.Remove(item);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
