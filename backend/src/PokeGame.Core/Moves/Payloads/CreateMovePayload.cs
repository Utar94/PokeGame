namespace PokeGame.Core.Moves.Payloads
{
  public class CreateMovePayload : SaveMovePayload
  {
    public PokemonType Type { get; set; }
    public MoveCategory Category { get; set; }
  }
}
