using PokeGame.Domain;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class MoveEntity : Entity
  {
    public PokemonType Type { get; set; }
    public MoveCategory Category { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public byte? Accuracy { get; set; }
    public byte? Power { get; set; }
    public byte PowerPoints { get; set; }

    public StatusCondition? StatusCondition { get; set; }
    public byte? StatusChance { get; set; }
    public string? StatisticStages { get; set; }
    public short AccuracyStage { get; set; }
    public short EvasionStage { get; set; }
    public string? VolatileConditions { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }

    public List<PokemonMoveEntity> PokemonMoves { get; set; } = new();

    public void Synchronize(Move move)
    {
      base.Synchronize(move);

      Type = move.Type;
      Category = move.Category;

      Name = move.Name;
      Description = move.Description;

      Accuracy = move.Accuracy;
      Power = move.Power;
      PowerPoints = move.PowerPoints;

      StatusCondition = move.StatusCondition;
      StatusChance = move.StatusChance;
      IEnumerable<KeyValuePair<Statistic, short>> statisticStages = move.StatisticStages.Where(x => x.Value != 0);
      StatisticStages = statisticStages.Any()
        ? string.Join('|', statisticStages.Select(pair => string.Join(':', pair.Key, pair.Value)))
        : null;
      AccuracyStage = move.AccuracyStage;
      EvasionStage = move.EvasionStage;
      VolatileConditions = move.VolatileConditions.Any()
        ? string.Join('|', move.VolatileConditions)
        : null;

      Notes = move.Notes;
      Reference = move.Reference;
    }
  }
}
