using PokeGame.Domain.Items;

namespace PokeGame.Domain.Trainers
{
  public class ItemPriceRequiredException : Exception
  {
    public ItemPriceRequiredException(Item item)
      : base($"The item '{item}' price is required.")
    {
      Data["Item"] = item ?? throw new ArgumentNullException(nameof(item));
    }
  }
}
