using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Species;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers;

internal static class PokemonSpeciesEvents
{
  public class PokemonSpeciesCreatedEventHandler : INotificationHandler<PokemonSpecies.CreatedEvent>
  {
    private readonly PokeGameContext _context;

    public PokemonSpeciesCreatedEventHandler(PokeGameContext context)
    {
      _context = context;
    }

    public async Task Handle(PokemonSpecies.CreatedEvent @event, CancellationToken cancellationToken)
    {
      PokemonSpeciesEntity? species = await _context.PokemonSpecies.AsNoTracking()
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
      if (species == null)
      {
        species = new(@event);

        _context.PokemonSpecies.Add(species);

        await _context.SaveChangesAsync(cancellationToken);
      }
    }
  }

  public class PokemonSpeciesDeletedEventHandler : INotificationHandler<PokemonSpecies.DeletedEvent>
  {
    private readonly PokeGameContext _context;

    public PokemonSpeciesDeletedEventHandler(PokeGameContext context)
    {
      _context = context;
    }

    public async Task Handle(PokemonSpecies.DeletedEvent @event, CancellationToken cancellationToken)
    {
      PokemonSpeciesEntity? species = await _context.PokemonSpecies
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
      if (species != null)
      {
        _context.PokemonSpecies.Remove(species);

        await _context.SaveChangesAsync(cancellationToken);
      }
    }
  }

  public class PokemonSpeciesUpdatedEventHandler : INotificationHandler<PokemonSpecies.UpdatedEvent>
  {
    private readonly PokeGameContext _context;

    public PokemonSpeciesUpdatedEventHandler(PokeGameContext context)
    {
      _context = context;
    }

    public async Task Handle(PokemonSpecies.UpdatedEvent @event, CancellationToken cancellationToken)
    {
      PokemonSpeciesEntity species = await _context.PokemonSpecies
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The Pokémon species entity 'AggregateId={@event.AggregateId}' could not be found.");

      species.Update(@event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
