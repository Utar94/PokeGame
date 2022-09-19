using PokeGame.Domain.Pokemon;

namespace PokeGame.Domain.Moves.Payloads
{
  public abstract class SaveMovePayload
  {
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public byte? Accuracy { get; set; }
    public byte? Power { get; set; }
    public byte PowerPoints { get; set; }

    public StatusCondition? StatusCondition { get; set; }
    public byte? StatusChance { get; set; }
    public IEnumerable<StatisticStagePayload>? StatisticStages { get; set; }
    public short AccuracyStage { get; set; }
    public short EvasionStage { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
