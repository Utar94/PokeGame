using PokeGame.Domain.Moves.Events;
using PokeGame.Domain.Moves.Payloads;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Domain.Moves
{
  public class Move : Aggregate
  {
    public Move(CreateMovePayload payload)
    {
      ApplyChange(new MoveCreated(payload));
    }
    private Move()
    {
    }

    public PokemonType Type { get; private set; }
    public MoveCategory Category { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public byte? Accuracy { get; private set; }
    public byte? Power { get; private set; }
    public byte PowerPoints { get; private set; }

    public StatusCondition? StatusCondition { get; private set; }
    public byte? StatusChance { get; private set; }
    public Dictionary<Statistic, short> StatisticStages { get; private set; } = new();
    public short AccuracyStage { get; private set; }
    public short EvasionStage { get; private set; }
    public HashSet<string> VolatileConditions { get; private set; } = new();

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public void Delete() => ApplyChange(new MoveDeleted());
    public void Update(UpdateMovePayload payload) => ApplyChange(new MoveUpdated(payload));

    protected virtual void Apply(MoveCreated @event)
    {
      Type = @event.Payload.Type;
      Category = @event.Payload.Category;

      Apply(@event.Payload);
    }
    protected virtual void Apply(MoveDeleted @event)
    {
      Delete(@event);
    }
    protected virtual void Apply(MoveUpdated @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SaveMovePayload payload)
    {
      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      Accuracy = payload.Accuracy;
      Power = payload.Power;
      PowerPoints = payload.PowerPoints;

      StatusCondition = payload.StatusCondition;
      StatusChance = payload.StatusCondition.HasValue ? payload.StatusChance : null;

      StatisticStages.Clear();
      if (payload.StatisticStages?.Any() == true)
      {
        foreach (StatisticStagePayload stage in payload.StatisticStages)
        {
          StatisticStages[stage.Statistic] = stage.Value;
        }
      }

      AccuracyStage = payload.AccuracyStage;
      EvasionStage = payload.EvasionStage;

      VolatileConditions.Clear();
      if (payload.VolatileConditions?.Any() == true)
      {
        foreach (string condition in payload.VolatileConditions)
        {
          if (!string.IsNullOrWhiteSpace(condition))
          {
            VolatileConditions.Add(condition.Trim());
          }
        }
      }

      Notes = payload.Notes?.CleanTrim();
      Reference = payload.Reference;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
