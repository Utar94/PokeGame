using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.EntityFrameworkCore.Entities;

internal class MoveEntity : AggregateEntity
{
  public int MoveId { get; private set; }
  public Guid Id { get; private set; }

  public PokemonType Type { get; private set; }
  public MoveCategory Category { get; private set; }
  public MoveKind? Kind { get; private set; }

  public string Name { get; private set; } = string.Empty;
  public string? Description { get; private set; }

  public int? Accuracy { get; private set; }
  public int? Power { get; private set; }
  public int PowerPoints { get; private set; }

  public int AttackStages { get; private set; }
  public int DefenseStages { get; private set; }
  public int SpecialAttackStages { get; private set; }
  public int SpecialDefenseStages { get; private set; }
  public int SpeedStages { get; private set; }
  public int AccuracyStages { get; private set; }
  public int EvasionStages { get; private set; }
  public StatusCondition? InflictedStatusCondition { get; private set; }
  public int? InflictedStatusChance { get; private set; }
  public string? VolatileConditions { get; private set; }

  public string? Link { get; private set; }
  public string? Notes { get; private set; }

  public MoveEntity(Move.CreatedEvent @event) : base(@event)
  {
    Id = @event.AggregateId.ToGuid();

    Type = @event.Type;
    Category = @event.Category;

    Name = @event.Name.Value;
  }

  private MoveEntity() : base()
  {
  }

  public void Update(Move.UpdatedEvent @event)
  {
    base.Update(@event);

    if (@event.Kind != null)
    {
      Kind = @event.Kind.Value;
    }

    if (@event.Name != null)
    {
      Name = @event.Name.Value;
    }
    if (@event.Description != null)
    {
      Description = @event.Description.Value?.Value;
    }

    if (@event.Accuracy != null)
    {
      Accuracy = @event.Accuracy.Value;
    }
    if (@event.Power != null)
    {
      Power = @event.Power.Value;
    }
    if (@event.PowerPoints.HasValue)
    {
      PowerPoints = @event.PowerPoints.Value;
    }

    SetStatisticChanges(@event.StatisticChanges);
    if (@event.Status != null)
    {
      SetInflictedStatus(@event.Status.Value);
    }
    SetVolatileConditions(@event.VolatileConditions);

    if (@event.Link != null)
    {
      Link = @event.Link.Value?.Value;
    }
    if (@event.Notes != null)
    {
      Notes = @event.Notes.Value?.Value;
    }
  }
  private void SetStatisticChanges(IEnumerable<KeyValuePair<BattleStatistic, int>> changes)
  {
    foreach (KeyValuePair<BattleStatistic, int> stages in changes)
    {
      switch (stages.Key)
      {
        case BattleStatistic.Attack:
          AttackStages = stages.Value;
          break;
        case BattleStatistic.Defense:
          DefenseStages = stages.Value;
          break;
        case BattleStatistic.SpecialAttack:
          SpecialAttackStages = stages.Value;
          break;
        case BattleStatistic.SpecialDefense:
          SpecialDefenseStages = stages.Value;
          break;
        case BattleStatistic.Speed:
          SpeedStages = stages.Value;
          break;
        case BattleStatistic.Accuracy:
          AccuracyStages = stages.Value;
          break;
        case BattleStatistic.Evasion:
          EvasionStages = stages.Value;
          break;
        default:
          throw new NotSupportedException($"The battle statistic '{stages.Key}' is not supported.");
      }
    }
  }
  private void SetInflictedStatus(IInflictedCondition? status)
  {
    InflictedStatusCondition = status?.Condition;
    InflictedStatusChance = status?.Chance;
  }
  private void SetVolatileConditions(IEnumerable<KeyValuePair<VolatileCondition, ActionKind>> changes)
  {
    HashSet<string> volatileConditions = new(GetVolatileConditions());
    foreach (KeyValuePair<VolatileCondition, ActionKind> volatileCondition in changes)
    {
      switch (volatileCondition.Value)
      {
        case ActionKind.Add:
          volatileConditions.Add(volatileCondition.Key.Value);
          break;
        case ActionKind.Remove:
          volatileConditions.Remove(volatileCondition.Key.Value);
          break;
        default:
          throw new ActionKindNotSupportedException(volatileCondition.Value);
      }
    }
    VolatileConditions = SerializeVolatileConditions(volatileConditions);
  }

  public IReadOnlyCollection<StatisticChangeModel> GetStatisticChanges()
  {
    List<StatisticChangeModel> changes = new(capacity: 7);
    if (AttackStages != 0)
    {
      changes.Add(new StatisticChangeModel(BattleStatistic.Attack, AttackStages));
    }
    if (DefenseStages != 0)
    {
      changes.Add(new StatisticChangeModel(BattleStatistic.Defense, DefenseStages));
    }
    if (SpecialAttackStages != 0)
    {
      changes.Add(new StatisticChangeModel(BattleStatistic.SpecialAttack, SpecialAttackStages));
    }
    if (SpecialDefenseStages != 0)
    {
      changes.Add(new StatisticChangeModel(BattleStatistic.SpecialDefense, SpecialDefenseStages));
    }
    if (SpeedStages != 0)
    {
      changes.Add(new StatisticChangeModel(BattleStatistic.Speed, SpeedStages));
    }
    if (AccuracyStages != 0)
    {
      changes.Add(new StatisticChangeModel(BattleStatistic.Accuracy, AccuracyStages));
    }
    if (EvasionStages != 0)
    {
      changes.Add(new StatisticChangeModel(BattleStatistic.Evasion, EvasionStages));
    }
    return changes.AsReadOnly();
  }
  public InflictedConditionModel? GetInflictedStatus()
  {
    return (InflictedStatusCondition.HasValue && InflictedStatusChance.HasValue)
      ? new InflictedConditionModel(InflictedStatusCondition.Value, InflictedStatusChance.Value)
      : null;
  }
  public IReadOnlyCollection<string> GetVolatileConditions()
  {
    return (VolatileConditions == null ? null : JsonSerializer.Deserialize<IReadOnlyCollection<string>>(VolatileConditions)) ?? [];
  }

  private static string? SerializeVolatileConditions(IEnumerable<string> volatileConditions)
  {
    return volatileConditions.Any() ? JsonSerializer.Serialize(volatileConditions) : null;
  }

  public override string ToString() => $"{Name} | {base.ToString()}";
}
