using PokeGame.Application.Models;
using PokeGame.Domain;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Application.Moves.Models
{
  public class MoveModel : AggregateModel
  {
    public PokemonType Type { get; set; }
    public MoveCategory Category { get; set; }
    public MoveKind? Kind { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public byte? Accuracy { get; set; }
    public byte? Power { get; set; }
    public byte PowerPoints { get; set; }

    public StatusCondition? StatusCondition { get; set; }
    public byte? StatusChance { get; set; }
    public IEnumerable<StatisticStageModel> StatisticStages { get; set; } = Enumerable.Empty<StatisticStageModel>();
    public short AccuracyStage { get; set; }
    public short EvasionStage { get; set; }
    public IEnumerable<string> VolatileConditions { get; set; } = Enumerable.Empty<string>();

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
