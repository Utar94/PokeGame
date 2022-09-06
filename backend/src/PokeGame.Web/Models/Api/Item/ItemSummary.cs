using PokeGame.Application.Items.Models;
using PokeGame.Domain.Items;

namespace PokeGame.Web.Models.Api.Items
{
  public class ItemSummary : AggregateSummary
  {
    public ItemSummary(ItemModel model) : base(model)
    {
      Category = model.Category;
      Price = model.Price;
      Name = model.Name;
    }

    public ItemCategory Category { get; set; }

    public int? Price { get; set; }

    public string Name { get; set; } = string.Empty;
  }
}
