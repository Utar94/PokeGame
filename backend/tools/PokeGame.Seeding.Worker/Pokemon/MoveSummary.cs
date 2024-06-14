using PokeGame.Contracts;
using PokeGame.Contracts.Moves;

namespace PokeGame.Seeding.Worker.Pokemon;

internal record MoveSummary(PokemonType Type, MoveCategory Category, string UniqueName, string? DisplayName, string? Description,
  int? Accuracy, int? Power, int PowerPoints, IEnumerable<StatisticChange> StatisticChanges, IEnumerable<InflictedStatusCondition> StatusConditions,
  string? Reference, string? Notes);
