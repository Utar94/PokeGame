using Logitar.Portal.Contracts.Search;

namespace PokeGame.Contracts.Abilities;

public record SearchAbilitiesPayload : SearchPayload
{
  public new List<AbilitySortOption> Sort { get; set; } = [];
}
