namespace PokeGame.Domain.Pokemon.Payloads
{
  public class UsePokemonMovePayload
  {
    public DamagePayload? Damage { get; set; }

    public StatusCondition? StatusCondition { get; set; }

    public IEnumerable<TargetPayload>? Targets { get; set; }
  }
}
