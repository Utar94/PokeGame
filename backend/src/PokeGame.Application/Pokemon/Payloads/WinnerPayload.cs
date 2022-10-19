namespace PokeGame.Application.Pokemon.Payloads
{
  public class WinnerPokemonPayload
  {
    public Guid Id { get; set; }

    public bool CanEvolve { get; set; }
    public bool HasParticipated { get; set; }

    public double? OtherModifiers { get; set; }
  }
}
