using PokeGame.Contracts.Abilities;

namespace PokeGame.Seeding.Worker.Backend;

internal record AbilitySummary(Guid Id, AbilityKind? Kind, string Name, string? Description, string? Link, string? Notes);
