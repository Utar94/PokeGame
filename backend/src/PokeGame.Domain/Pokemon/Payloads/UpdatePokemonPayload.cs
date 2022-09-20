namespace PokeGame.Domain.Pokemon.Payloads
{
  public class UpdatePokemonPayload : SavePokemonPayload
  {
    public byte Friendship { get; set; }

    public ushort CurrentHitPoints { get; set; }
  }
}
