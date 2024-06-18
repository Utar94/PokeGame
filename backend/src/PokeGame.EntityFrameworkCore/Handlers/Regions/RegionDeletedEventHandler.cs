using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Regions.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers.Regions;

internal class RegionDeletedEventHandler : INotificationHandler<RegionDeletedEvent>
{
  private readonly PokemonContext _context;

  public RegionDeletedEventHandler(PokemonContext context)
  {
    _context = context;
  }

  public async Task Handle(RegionDeletedEvent @event, CancellationToken cancellationToken)
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
