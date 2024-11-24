namespace PokeGame.Domain.Species;

public interface IPokemonSpeciesRepository
{
  Task<PokemonSpecies?> LoadAsync(SpeciesId id, CancellationToken cancellationToken = default);
  Task<PokemonSpecies?> LoadAsync(SpeciesId id, long? version, CancellationToken cancellationToken = default);

  Task<IReadOnlyCollection<PokemonSpecies>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IReadOnlyCollection<PokemonSpecies>> LoadAsync(IEnumerable<SpeciesId> ids, CancellationToken cancellationToken = default);

  Task SaveAsync(PokemonSpecies species, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<PokemonSpecies> species, CancellationToken cancellationToken = default);
}
