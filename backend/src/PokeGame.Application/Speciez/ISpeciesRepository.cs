using PokeGame.Domain.Speciez;

namespace PokeGame.Application.Speciez;

public interface ISpeciesRepository
{
  Task<Species?> LoadAsync(SpeciesId id, CancellationToken cancellationToken = default);
  Task<Species?> LoadAsync(SpeciesId id, long? version, CancellationToken cancellationToken = default);

  Task<IReadOnlyCollection<Species>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IReadOnlyCollection<Species>> LoadAsync(IEnumerable<SpeciesId> ids, CancellationToken cancellationToken = default);

  Task SaveAsync(Species species, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<Species> species, CancellationToken cancellationToken = default);
}
