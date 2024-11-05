using PokeGame.Contracts.Abilities;

namespace PokeGame.Seeding.Worker.Backend;

internal record AbilityPayload : CreateOrReplaceAbilityPayload
{
  public Guid Id { get; set; }
}
