using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Speciez.Events;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure.Handlers;

internal class SpeciesEvents : INotificationHandler<SpeciesCreated>,
  INotificationHandler<SpeciesDeleted>,
  INotificationHandler<SpeciesRegionalNumberChanged>,
  INotificationHandler<SpeciesUpdated>
{
  private readonly PokeGameContext _context;

  public SpeciesEvents(PokeGameContext context)
  {
    _context = context;
  }

  public async Task Handle(SpeciesCreated @event, CancellationToken cancellationToken)
  {
    SpeciesEntity? species = await _context.Species.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (species == null)
    {
      species = new(@event);

      _context.Species.Add(species);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(SpeciesDeleted @event, CancellationToken cancellationToken)
  {
    SpeciesEntity? species = await _context.Species
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (species != null)
    {
      _context.Species.Remove(species);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(SpeciesRegionalNumberChanged @event, CancellationToken cancellationToken)
  {
    SpeciesEntity species = await _context.Species
      .Include(x => x.RegionalSpecies)
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The species entity 'StreamId={@event.StreamId}' could not be found.");

    RegionEntity region = await _context.Regions
      .SingleOrDefaultAsync(x => x.StreamId == @event.RegionId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The region entity 'StreamId={@event.RegionId}' could not be found.");

    species.SetRegionalNumber(region, @event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(SpeciesUpdated @event, CancellationToken cancellationToken)
  {
    SpeciesEntity species = await _context.Species
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The species entity 'StreamId={@event.StreamId}' could not be found.");

    species.Update(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
