using Logitar.Identity.Domain.Shared;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities;

public interface IAbilityRepository
{
  Task<IReadOnlyCollection<AbilityAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  Task<AbilityAggregate?> LoadAsync(AbilityId id, CancellationToken cancellationToken = default);
  Task<AbilityAggregate?> LoadAsync(AbilityId id, long? version, CancellationToken cancellationToken = default);
  Task<AbilityAggregate?> LoadAsync(UniqueNameUnit uniqueName, CancellationToken cancellationToken = default);

  Task SaveAsync(AbilityAggregate ability, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<AbilityAggregate> abilities, CancellationToken cancellationToken = default);
}
