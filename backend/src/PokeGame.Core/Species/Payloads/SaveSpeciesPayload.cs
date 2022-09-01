using PokeGame.Core.Pokemon;

namespace PokeGame.Core.Species.Payloads
{
  public abstract class SaveSpeciesPayload
  {
    public string Name { get; set; } = null!;
    public string? Category { get; set; }
    public string? Description { get; set; }

    public double? GenderRatio { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }

    public int? BaseExperienceYield { get; set; }
    public byte BaseFriendship { get; set; }
    public byte? CatchRate { get; set; }
    public LevelingRate LevelingRate { get; set; }

    public IEnumerable<StatisticValuePayload>? BaseStatistics { get; set; }
    public IEnumerable<StatisticValuePayload>? EvYield { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }

    public IEnumerable<Guid>? AbilityIds { get; set; }
  }
}
