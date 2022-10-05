using PokeGame.Application.Items.Models;

namespace PokeGame.Web.Models.Api.Game
{
  public class ItemSummary
  {
    public ItemSummary(ItemModel item)
    {
      Name = item.Name;
      Description = item.Description;

      Picture = item.Picture;
    }

    public string Name { get; set; }
    public string? Description { get; set; }

    public string? Picture { get; set; }
  }
}
