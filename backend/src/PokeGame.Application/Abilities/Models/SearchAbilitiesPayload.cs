using Logitar.Portal.Contracts.Search;

namespace PokeGame.Application.Abilities.Models;

public record SearchAbilitiesPayload : SearchPayload
{
  public new List<AbilitySortOption> Sort { get; set; } = [];
}
