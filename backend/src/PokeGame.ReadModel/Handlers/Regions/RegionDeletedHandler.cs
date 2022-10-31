using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Regions.Events;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Handlers.Regions
{
  internal class RegionDeletedHandler : INotificationHandler<RegionDeleted>
  {
    private readonly ReadContext _readContext;

    public RegionDeletedHandler(ReadContext readContext)
    {
      _readContext = readContext;
    }

    public async Task Handle(RegionDeleted notification, CancellationToken cancellationToken)
    {
      RegionEntity? region = await _readContext.Regions
        .SingleOrDefaultAsync(x => x.Id == notification.AggregateId, cancellationToken);

      if (region != null)
      {
        _readContext.Regions.Remove(region);
        await _readContext.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
