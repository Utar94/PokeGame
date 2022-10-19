namespace PokeGame.Domain.Items.Payloads
{
  public class CreateItemPayload : SaveItemPayload
  {
    public ItemCategory Category { get; set; }
  }
}
