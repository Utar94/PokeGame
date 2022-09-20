namespace PokeGame.Domain.Pokemon.Payloads
{
  public class UpdatePokemonConditionPayload
  {
    public ushort CurrentHitPoints { get; set; }
    public StatusCondition? StatusCondition { get; set; }
  }
}
