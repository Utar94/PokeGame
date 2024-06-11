using Logitar.Portal.Contracts.Search;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities;

public interface IAbilityQuerier
{
  Task<Ability> ReadAsync(AbilityAggregate ability, CancellationToken cancellationToken = default);
  Task<Ability?> ReadAsync(AbilityId id, CancellationToken cancellationToken = default);
  Task<Ability?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<Ability?> ReadAsync(string uniqueName, CancellationToken cancellationToken = default);

  Task<SearchResults<Ability>> SearchAsync(SearchAbilitiesPayload payload, CancellationToken cancellationToken = default);
}
