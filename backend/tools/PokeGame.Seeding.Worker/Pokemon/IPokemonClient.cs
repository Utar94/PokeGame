using Logitar.Portal.Contracts.Search;
using PokeGame.Contracts.Abilities;
using PokeGame.Contracts.Moves;
using PokeGame.Contracts.Regions;

namespace PokeGame.Seeding.Worker.Pokemon;

public interface IPokemonClient
{
  Task<Ability> CreateAbilityAsync(CreateAbilityPayload payload, CancellationToken cancellationToken = default);
  Task<Move> CreateMoveAsync(CreateMovePayload payload, CancellationToken cancellationToken = default);
  Task<Region> CreateRegionAsync(CreateRegionPayload payload, CancellationToken cancellationToken = default);

  Task<Ability?> ReplaceAbilityAsync(Guid id, ReplaceAbilityPayload payload, long? version = null, CancellationToken cancellationToken = default);
  Task<Move?> ReplaceMoveAsync(Guid id, ReplaceMovePayload payload, long? version = null, CancellationToken cancellationToken = default);
  Task<Region?> ReplaceRegionAsync(Guid id, ReplaceRegionPayload payload, long? version = null, CancellationToken cancellationToken = default);

  Task<SearchResults<Ability>> SearchAbilitiesAsync(SearchAbilitiesPayload payload, CancellationToken cancellationToken = default);
  Task<SearchResults<Move>> SearchMovesAsync(SearchMovesPayload payload, CancellationToken cancellationToken = default);
  Task<SearchResults<Region>> SearchRegionsAsync(SearchRegionsPayload payload, CancellationToken cancellationToken = default);
}
