using Logitar.EventSourcing;
using MediatR;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;

namespace PokeGame.Domain.Moves;

public class Move : AggregateRoot
{
  private const int PowerMaximumValue = 250;
  private const int PowerPointsMaximumValue = 40;
  private const int StageMaximumValue = 6;
  private const int StageMinimumValue = -6;

  private UpdatedEvent _updatedEvent = new();

  public new MoveId Id => new(base.Id);

  public PokemonType Type { get; private set; }
  public MoveCategory Category { get; private set; }
  private MoveKind? _kind = null;
  public MoveKind? Kind
  {
    get => _kind;
    set
    {
      if (_kind != value)
      {
        if (value.HasValue && !Enum.IsDefined(value.Value))
        {
          throw new ArgumentOutOfRangeException(nameof(Kind));
        }

        _kind = value;
        _updatedEvent.Kind = new Change<MoveKind?>(value);
      }
    }
  }

  private Name? _name = null;
  public Name Name
  {
    get => _name ?? throw new InvalidOperationException($"The {nameof(Name)} has not been initialized yet.");
    set
    {
      if (_name != value)
      {
        _name = value;
        _updatedEvent.Name = value;
      }
    }
  }
  private Description? _description = null;
  public Description? Description
  {
    get => _description;
    set
    {
      if (_description != value)
      {
        _description = value;
        _updatedEvent.Description = new Change<Description>(value);
      }
    }
  }

  private int? _accuracy = null;
  public int? Accuracy
  {
    get => _accuracy;
    set
    {
      if (value < 1 || value > 100)
      {
        throw new ArgumentOutOfRangeException(nameof(Accuracy));
      }

      if (_accuracy != value)
      {
        _accuracy = value;
        _updatedEvent.Accuracy = new Change<int?>(value);
      }
    }
  }
  private int? _power = null;
  public int? Power
  {
    get => _power;
    set
    {
      if (value < 1 || value > PowerMaximumValue)
      {
        throw new ArgumentOutOfRangeException(nameof(Power));
      }
      else if (Category == MoveCategory.Special && value.HasValue)
      {
        throw new ArgumentException($"A move belonging to the '{nameof(MoveCategory.Special)}' category should not have a power value.", nameof(Power));
      }

      if (_power != value)
      {
        _power = value;
        _updatedEvent.Power = new Change<int?>(value);
      }
    }
  }
  private int _powerPoints = 0;
  public int PowerPoints
  {
    get => _powerPoints;
    set
    {
      if (value < 1 || value > PowerPointsMaximumValue)
      {
        throw new ArgumentOutOfRangeException(nameof(PowerPoints));
      }

      if (_powerPoints != value)
      {
        _powerPoints = value;
        _updatedEvent.PowerPoints = value;
      }
    }
  }

  private readonly Dictionary<BattleStatistic, int> _statisticChanges = [];
  public IReadOnlyDictionary<BattleStatistic, int> StatisticChanges => _statisticChanges.AsReadOnly();
  private InflictedCondition? _status = null;
  public InflictedCondition? Status
  {
    get => _status;
    set
    {
      if (_status != value)
      {
        _status = value;
        _updatedEvent.Status = new Change<InflictedCondition>(value);
      }
    }
  }
  private readonly HashSet<VolatileCondition> _volatileConditions = [];
  public IReadOnlyCollection<VolatileCondition> VolatileConditions => _volatileConditions.ToArray().AsReadOnly();
  public bool HasVolatileCondition(VolatileCondition volatileCondition) => _volatileConditions.Contains(volatileCondition);

  private Url? _link = null;
  public Url? Link
  {
    get => _link;
    set
    {
      if (_link != value)
      {
        _link = value;
        _updatedEvent.Link = new Change<Url>(value);
      }
    }
  }
  private Notes? _notes = null;
  public Notes? Notes
  {
    get => _notes;
    set
    {
      if (_notes != value)
      {
        _notes = value;
        _updatedEvent.Notes = new Change<Notes>(value);
      }
    }
  }

  public Move() : base()
  {
  }

  public Move(PokemonType type, MoveCategory category, Name name, UserId userId, MoveId? id = null) : base((id ?? MoveId.NewId()).AggregateId)
  {
    if (!Enum.IsDefined(type))
    {
      throw new ArgumentOutOfRangeException(nameof(type));
    }
    if (!Enum.IsDefined(category))
    {
      throw new ArgumentOutOfRangeException(nameof(category));
    }

    Raise(new CreatedEvent(type, category, name), userId.ActorId);
  }
  protected virtual void Apply(CreatedEvent @event)
  {
    Type = @event.Type;
    Category = @event.Category;

    _name = @event.Name;
  }

  public void Delete(UserId userId)
  {
    if (!IsDeleted)
    {
      Raise(new DeletedEvent(), userId.ActorId);
    }
  }

  public void AddVolatileCondition(VolatileCondition volatileCondition)
  {
    if (_volatileConditions.Add(volatileCondition))
    {
      _updatedEvent.VolatileConditions[volatileCondition] = ActionKind.Add;
    }
  }
  public void RemoveVolatileCondition(VolatileCondition volatileCondition)
  {
    if (_volatileConditions.Remove(volatileCondition))
    {
      _updatedEvent.VolatileConditions[volatileCondition] = ActionKind.Remove;
    }
  }

  public void SetStatisticChange(BattleStatistic statistic, int stages)
  {
    if (!Enum.IsDefined(statistic))
    {
      throw new ArgumentOutOfRangeException(nameof(statistic));
    }
    if (stages < StageMinimumValue || stages > StageMaximumValue)
    {
      throw new ArgumentOutOfRangeException(nameof(stages));
    }

    if (!_statisticChanges.TryGetValue(statistic, out int existingStages) || existingStages != stages)
    {
      if (stages == 0)
      {
        _statisticChanges.Remove(statistic);
      }
      else
      {
        _statisticChanges[statistic] = stages;
      }
      _updatedEvent.StatisticChanges[statistic] = stages;
    }
  }

  public void Update(UserId userId)
  {
    if (_updatedEvent.HasChanges)
    {
      Raise(_updatedEvent, userId.ActorId, DateTime.Now);
      _updatedEvent = new();
    }
  }
  protected virtual void Apply(UpdatedEvent @event)
  {
    if (@event.Kind != null)
    {
      _kind = @event.Kind.Value;
    }

    if (@event.Name != null)
    {
      _name = @event.Name;
    }
    if (@event.Description != null)
    {
      _description = @event.Description.Value;
    }

    if (@event.Accuracy != null)
    {
      _accuracy = @event.Accuracy.Value;
    }
    if (@event.Power != null)
    {
      _power = @event.Power.Value;
    }
    if (@event.PowerPoints != null)
    {
      _powerPoints = @event.PowerPoints.Value;
    }

    foreach (KeyValuePair<BattleStatistic, int> statisticChange in @event.StatisticChanges)
    {
      if (statisticChange.Value == 0)
      {
        _statisticChanges.Remove(statisticChange.Key);
      }
      else
      {
        _statisticChanges[statisticChange.Key] = statisticChange.Value;
      }
    }
    if (@event.Status != null)
    {
      _status = @event.Status.Value;
    }
    foreach (KeyValuePair<VolatileCondition, ActionKind> volatileCondition in @event.VolatileConditions)
    {
      switch (volatileCondition.Value)
      {
        case ActionKind.Add:
          _volatileConditions.Add(volatileCondition.Key);
          break;
        case ActionKind.Remove:
          _volatileConditions.Remove(volatileCondition.Key);
          break;
        default:
          throw new ActionKindNotSupportedException(volatileCondition.Value);
      }
    }

    if (@event.Link != null)
    {
      _link = @event.Link.Value;
    }
    if (@event.Notes != null)
    {
      _notes = @event.Notes.Value;
    }
  }

  public override string ToString() => $"{Name} | {base.ToString()}";

  public class CreatedEvent : DomainEvent, INotification
  {
    public PokemonType Type { get; }
    public MoveCategory Category { get; }

    public Name Name { get; }

    public CreatedEvent(PokemonType type, MoveCategory category, Name name)
    {
      Type = type;
      Category = category;

      Name = name;
    }
  }

  public class DeletedEvent : DomainEvent, INotification
  {
    public DeletedEvent()
    {
      IsDeleted = true;
    }
  }

  public class UpdatedEvent : DomainEvent, INotification
  {
    public Change<MoveKind?>? Kind { get; set; }

    public Name? Name { get; set; }
    public Change<Description>? Description { get; set; }

    public Change<int?>? Accuracy { get; set; }
    public Change<int?>? Power { get; set; }
    public int? PowerPoints { get; set; }

    public Dictionary<BattleStatistic, int> StatisticChanges { get; set; } = [];
    public Change<InflictedCondition>? Status { get; set; }
    public Dictionary<VolatileCondition, ActionKind> VolatileConditions { get; set; } = [];

    public Change<Url>? Link { get; set; }
    public Change<Notes>? Notes { get; set; }

    public bool HasChanges => Kind != null || Name != null || Description != null
      || Accuracy != null || Power != null || PowerPoints != null
      || StatisticChanges.Count > 0 || Status != null || VolatileConditions.Count > 0
      || Link != null || Notes != null;
  }
}
