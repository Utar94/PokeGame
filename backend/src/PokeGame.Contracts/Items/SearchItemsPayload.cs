using Logitar.Portal.Contracts.Search;

namespace PokeGame.Contracts.Items;

public record SearchItemsPayload : SearchPayload
{
  public ItemCategory? Category { get; set; }

  public new List<ItemSortOption> Sort { get; set; } = [];
}
