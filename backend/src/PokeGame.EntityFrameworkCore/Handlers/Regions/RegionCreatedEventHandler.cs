using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Regions.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers.Regions;

internal class RegionCreatedEventHandler : INotificationHandler<RegionCreatedEvent>
{
  private readonly PokemonContext _context;

  public RegionCreatedEventHandler(PokemonContext context)
  {
    _context = context;
  }

  public async Task Handle(RegionCreatedEvent @event, CancellationToken cancellationToken)
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
