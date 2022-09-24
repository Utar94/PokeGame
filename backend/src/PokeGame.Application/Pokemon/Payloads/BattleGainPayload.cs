namespace PokeGame.Application.Pokemon.Payloads
{
  public class BattleGainPayload
  {
    public Guid DefeatedId { get; set; }
    public bool IsTrainerBattle { get; set; }

    public IEnumerable<WinnerPokemonPayload>? Winners { get; set; }
  }
}
