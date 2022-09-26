namespace PokeGame.Domain.Pokemon.Payloads
{
  public class HealPokemonPayload
  {
    public bool RestoreAllPowerPoints { get; set; }
    public ushort RestoreHitPoints { get; set; }

    public bool RemoveAllConditions { get; set; }
    public StatusCondition? StatusCondition { get; set; }
  }
}
