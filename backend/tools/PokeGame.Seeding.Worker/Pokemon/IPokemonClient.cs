using Logitar.Portal.Contracts.Search;
using PokeGame.Contracts.Abilities;

namespace PokeGame.Seeding.Worker.Pokemon;

public interface IPokemonClient
{
  Task<Ability> CreateAbilityAsync(CreateAbilityPayload payload, CancellationToken cancellationToken = default);
  Task<Ability?> ReplaceAbilityAsync(Guid id, ReplaceAbilityPayload payload, long? version = null, CancellationToken cancellationToken = default);
  Task<SearchResults<Ability>> SearchAbilitiesAsync(SearchAbilitiesPayload payload, CancellationToken cancellationToken = default);
}
