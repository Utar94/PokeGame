using PokeGame.Core.Items;
using System.Net;

namespace PokeGame.Core.Trainers
{
  public class ItemPriceRequiredException : ApiException
  {
    public ItemPriceRequiredException(Item item)
      : base(HttpStatusCode.BadRequest, $"The item '{item}' price is required.")
    {
      Item = item ?? throw new ArgumentNullException(nameof(item));
      Value = new { code = nameof(ItemPriceRequiredException).Remove(nameof(Exception)) };
    }

    public Item Item { get; }
  }
}
