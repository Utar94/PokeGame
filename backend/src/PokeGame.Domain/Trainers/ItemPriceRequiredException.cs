using PokeGame.Domain.Items;

namespace PokeGame.Domain.Trainers
{
  public class ItemPriceRequiredException : Exception
  {
    public ItemPriceRequiredException(Item item)
      : base($"The item '{item}' price is required.")
    {
      Data["ItemId"] = item.Id;
    }
  }
}
