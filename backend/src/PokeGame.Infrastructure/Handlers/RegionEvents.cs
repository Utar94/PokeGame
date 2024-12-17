using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Regions.Events;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure.Handlers;

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
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (region == null)
    {
      region = new(@event);

      _context.Regions.Add(region);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(RegionDeleted @event, CancellationToken cancellationToken)
  {
    RegionEntity? region = await _context.Regions
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (region != null)
    {
      _context.Regions.Remove(region);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(RegionUpdated @event, CancellationToken cancellationToken)
  {
    RegionEntity region = await _context.Regions
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The region entity 'StreamId={@event.StreamId}' could not be found.");

    region.Update(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
