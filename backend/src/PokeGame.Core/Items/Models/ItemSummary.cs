using PokeGame.Core.Models;

namespace PokeGame.Core.Items.Models
{
  public class ItemSummary : AggregateSummary
  {
    public ItemCategory Category { get; set; }

    public int? Price { get; set; }

    public string Name { get; set; } = null!;
  }
}
