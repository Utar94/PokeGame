using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Items.Events;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Items
{
  internal class ItemDeletedHandler : INotificationHandler<ItemDeleted>
  {
    private readonly ReadContext _readContext;

    public ItemDeletedHandler(ReadContext readContext)
    {
      _readContext = readContext;
    }

    public async Task Handle(ItemDeleted notification, CancellationToken cancellationToken)
    {
      ItemEntity? item = await _readContext.Items
        .SingleOrDefaultAsync(x => x.Id == notification.AggregateId, cancellationToken);

      if (item != null)
      {
        _readContext.Items.Remove(item);
        await _readContext.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
