namespace PokeGame.Core.Moves.Payloads
{
  public class CreateMovePayload : SaveMovePayload
  {
    public PokemonType Type { get; set; }
    public Category Category { get; set; }
  }
}
