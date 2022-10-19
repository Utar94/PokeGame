using PokeGame.Domain;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Pokemon;

namespace PokeGame.ReadModel.Entities
{
  internal class MoveEntity : Entity
  {
    public PokemonType Type { get; private set; }
    public MoveCategory Category { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public byte? Accuracy { get; private set; }
    public byte? Power { get; private set; }
    public byte PowerPoints { get; private set; }

    public StatusCondition? StatusCondition { get; private set; }
    public byte? StatusChance { get; private set; }
    public string? StatisticStages { get; private set; }
    public short AccuracyStage { get; private set; }
    public short EvasionStage { get; private set; }
    public string? VolatileConditions { get; private set; }

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public List<EvolutionEntity> Evolutions { get; private set; } = new();
    public List<PokemonMoveEntity> PokemonMoves { get; private set; } = new();

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
