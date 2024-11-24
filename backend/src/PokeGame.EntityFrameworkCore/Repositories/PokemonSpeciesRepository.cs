using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using PokeGame.Domain.Species;

namespace PokeGame.EntityFrameworkCore.Repositories;

internal class PokemonSpeciesRepository : Logitar.EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IPokemonSpeciesRepository
{
  public PokemonSpeciesRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer)
    : base(eventBus, eventContext, eventSerializer)
  {
  }

  public async Task<PokemonSpecies?> LoadAsync(SpeciesId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<PokemonSpecies?> LoadAsync(SpeciesId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync<PokemonSpecies>(id.AggregateId, version, cancellationToken);
  }

  public async Task<IReadOnlyCollection<PokemonSpecies>> LoadAsync(CancellationToken cancellationToken)
  {
    return (await LoadAsync<PokemonSpecies>(cancellationToken)).ToArray().AsReadOnly();
  }
  public async Task<IReadOnlyCollection<PokemonSpecies>> LoadAsync(IEnumerable<SpeciesId> ids, CancellationToken cancellationToken)
  {
    IEnumerable<AggregateId> aggregateIds = ids.Select(id => id.AggregateId);
    return (await LoadAsync<PokemonSpecies>(aggregateIds, cancellationToken)).ToArray().AsReadOnly();
  }

  public async Task SaveAsync(PokemonSpecies species, CancellationToken cancellationToken)
  {
    await base.SaveAsync(species, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<PokemonSpecies> species, CancellationToken cancellationToken)
  {
    await base.SaveAsync(species, cancellationToken);
  }
}
