using PokeGame.Application.Inventories.Models;
using PokeGame.Application.Items.Models;

namespace PokeGame.Web.Models.Api.Game
{
  public class GameInventoryLineModel
  {
    public GameInventoryLineModel(InventoryModel model)
    {
      ItemModel item = model.Item ?? throw new ArgumentException($"The {nameof(model.Item)} is required.", nameof(model));

      Id = item.Id;

      Name = item.Name;
      Description = item.Description;

      Picture = item.Picture;

      Quantity = model.Quantity;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }

    public string? Picture { get; set; }

    public int Quantity { get; set; }
  }
}
