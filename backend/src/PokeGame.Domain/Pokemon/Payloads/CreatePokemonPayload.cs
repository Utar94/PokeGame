using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Domain.Pokemon.Payloads
{
  public class CreatePokemonPayload
  {
    public Guid SpeciesId { get; set; }
    public Guid AbilityId { get; set; }

    public byte Level { get; set; }
    public int? Experience { get; set; }
    public byte? Friendship { get; set; }

    public PokemonGender Gender { get; set; }
    public string Nature { get; set; } = string.Empty;
    public string? Surname { get; set; }
    public string? Description { get; set; }

    public IEnumerable<StatisticValuePayload>? IndividualValues { get; set; }
    public IEnumerable<StatisticValuePayload>? EffortValues { get; set; }

    public ushort? CurrentHitPoints { get; set; }
    public StatusCondition? StatusCondition { get; set; }

    public IEnumerable<PokemonMovePayload>? Moves { get; set; }
    public Guid? HeldItemId { get; set; }

    public HistoryPayload? History { get; set; }
    public byte? Position { get; set; }
    public byte? Box { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
