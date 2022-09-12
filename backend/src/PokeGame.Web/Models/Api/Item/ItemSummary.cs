using PokeGame.Application.Items.Models;
using PokeGame.Domain.Items;

namespace PokeGame.Web.Models.Api.Items
{
  public class ItemSummary : AggregateSummary
  {
    public ItemSummary(ItemModel model) : base(model)
    {
      Category = model.Category;
      DefaultModifier = model.DefaultModifier;

      Price = model.Price;

      Name = model.Name;
      Description = model.Description;

      Notes = model.Notes;
      Reference = model.Reference;
    }

    public ItemCategory Category { get; set; }
    public double? DefaultModifier { get; set; }

    public int? Price { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
