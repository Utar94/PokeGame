using PokeGame.Core.Models;

namespace PokeGame.Core.Items.Models
{
  public class ItemModel : AggregateModel
  {
    public Category Category { get; set; }

    public int? Price { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
