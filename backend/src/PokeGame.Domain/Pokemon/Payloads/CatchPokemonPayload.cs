namespace PokeGame.Domain.Pokemon.Payloads
{
  public class CatchPokemonPayload
  {
    public HealPokemonPayload? Heal { get; set; }

    public byte? Friendship { get; set; }
    public string? Surname { get; set; }

    public string Location { get; set; } = string.Empty;
    public Guid TrainerId { get; set; }
  }
}
