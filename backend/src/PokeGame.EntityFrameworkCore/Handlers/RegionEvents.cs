using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Regions.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers;

internal class RegionEvents : INotificationHandler<RegionCreated>, INotificationHandler<RegionDeleted>, INotificationHandler<RegionUpdated>
{
  private readonly PokeGameContext _context;

  public RegionEvents(PokeGameContext context)
  {
    _context = context;
  }

  public async Task Handle(RegionCreated @event, CancellationToken cancellationToken)
  {
    RegionEntity? region = await _context.Regions.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
    if (region == null)
    {
      region = new RegionEntity(@event);

      _context.Regions.Add(region);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(RegionDeleted @event, CancellationToken cancellationToken)
  {
    RegionEntity? region = await _context.Regions
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
    if (region != null)
    {
      _context.Regions.Remove(region);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(RegionUpdated @event, CancellationToken cancellationToken)
  {
    RegionEntity region = await _context.Regions
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The region entity 'AggregateId={@event.AggregateId}' could not be found.");

    region.Update(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
