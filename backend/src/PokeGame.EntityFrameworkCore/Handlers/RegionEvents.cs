using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Regions;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers;

internal static class RegionEvents
{
  public class RegionCreatedEventHandler : INotificationHandler<Region.CreatedEvent>
  {
    private readonly PokeGameContext _context;

    public RegionCreatedEventHandler(PokeGameContext context)
    {
      _context = context;
    }

    public async Task Handle(Region.CreatedEvent @event, CancellationToken cancellationToken)
    {
      RegionEntity? region = await _context.Regions.AsNoTracking()
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
      if (region == null)
      {
        region = new(@event);

        _context.Regions.Add(region);

        await _context.SaveChangesAsync(cancellationToken);
      }
    }
  }

  public class RegionDeletedEventHandler : INotificationHandler<Region.DeletedEvent>
  {
    private readonly PokeGameContext _context;

    public RegionDeletedEventHandler(PokeGameContext context)
    {
      _context = context;
    }

    public async Task Handle(Region.DeletedEvent @event, CancellationToken cancellationToken)
    {
      RegionEntity? region = await _context.Regions
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
      if (region != null)
      {
        _context.Regions.Remove(region);

        await _context.SaveChangesAsync(cancellationToken);
      }
    }
  }

  public class RegionUpdatedEventHandler : INotificationHandler<Region.UpdatedEvent>
  {
    private readonly PokeGameContext _context;

    public RegionUpdatedEventHandler(PokeGameContext context)
    {
      _context = context;
    }

    public async Task Handle(Region.UpdatedEvent @event, CancellationToken cancellationToken)
    {
      RegionEntity region = await _context.Regions
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The region entity 'AggregateId={@event.AggregateId}' could not be found.");

      region.Update(@event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
