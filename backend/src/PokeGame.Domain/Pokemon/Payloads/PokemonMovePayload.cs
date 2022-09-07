namespace PokeGame.Domain.Pokemon.Payloads
{
  public class PokemonMovePayload
  {
    public Guid MoveId { get; set; }
    public byte Position { get; set; }
    public byte RemainingPowerPoints { get; set; }
  }
}
