using PokeGame.Application.Models;
using PokeGame.Domain.Items;

namespace PokeGame.Application.Items.Models
{
  public class ItemModel : AggregateModel
  {
    public ItemCategory Category { get; set; }
    public ItemType? Type { get; set; }
    public double? DefaultModifier { get; set; }

    public int? Price { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Picture { get; set; }
    public string? Reference { get; set; }
  }
}
