namespace PokeGame.Domain.Pokemon.Payloads
{
  public class HealPokemonPayload
  {
    public short RestoreHitPoints { get; set; }

    public bool RemoveAllConditions { get; set; }
    public StatusCondition? StatusCondition { get; set; }
  }
}
