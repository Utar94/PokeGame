using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities;

public interface IAbilityRepository
{
  Task<Ability?> LoadAsync(AbilityId id, CancellationToken cancellationToken = default);
  Task<Ability?> LoadAsync(AbilityId id, long? version, CancellationToken cancellationToken = default);

  Task<IReadOnlyCollection<Ability>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IReadOnlyCollection<Ability>> LoadAsync(IEnumerable<AbilityId> ids, CancellationToken cancellationToken = default);

  Task SaveAsync(Ability ability, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<Ability> abilities, CancellationToken cancellationToken = default);
}
