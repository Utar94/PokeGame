namespace PokeGame.Core.Items.Payloads
{
  public class CreateItemPayload : SaveItemPayload
  {
    public Category Category { get; set; }
  }
}
