using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Regions.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers.Regions;

internal class RegionUpdatedEventHandler : INotificationHandler<RegionUpdatedEvent>
{
  private readonly PokemonContext _context;

  public RegionUpdatedEventHandler(PokemonContext context)
  {
    _context = context;
  }

  public async Task Handle(RegionUpdatedEvent @event, CancellationToken cancellationToken)
  {
    RegionEntity? region = await _context.Regions
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The region entity 'AggregateId={@event.AggregateId}' could not be found.");

    region.Update(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
