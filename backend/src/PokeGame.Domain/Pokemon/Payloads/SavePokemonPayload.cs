using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Domain.Pokemon.Payloads
{
  public abstract class SavePokemonPayload
  {
    public string? Surname { get; set; }
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }

    public IEnumerable<StatisticValuePayload>? EffortValues { get; set; }

    public StatusCondition? StatusCondition { get; set; }
  }
}
