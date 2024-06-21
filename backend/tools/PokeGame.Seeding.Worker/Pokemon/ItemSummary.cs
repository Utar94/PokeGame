using PokeGame.Contracts.Items;
using PokeGame.Contracts.Items.Properties;

namespace PokeGame.Seeding.Worker.Pokemon;

internal record ItemSummary(ItemCategory Category, int? Price, string UniqueName, string? DisplayName, string? Description, string? Picture,
  MedicineProperties? Medicine, PokeBallProperties? PokeBall, string? Reference, string? Notes);
