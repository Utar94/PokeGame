using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Items.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers.Items;

internal class MedicinePropertiesChangedEventHandler : INotificationHandler<MedicinePropertiesChangedEvent>
{
  private readonly PokemonContext _context;

  public MedicinePropertiesChangedEventHandler(PokemonContext context)
  {
    _context = context;
  }

  public async Task Handle(MedicinePropertiesChangedEvent @event, CancellationToken cancellationToken)
  {
    ItemEntity? item = await _context.Items
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The item entity 'AggregateId={@event.AggregateId}' could not be found.");

    item.SetProperties(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
