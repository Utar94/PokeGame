namespace PokeGame.Core.Items.Payloads
{
  public class CreateItemPayload : SaveItemPayload
  {
    public ItemCategory Category { get; set; }
  }
}
