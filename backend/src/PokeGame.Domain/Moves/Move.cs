﻿using Logitar.EventSourcing;
using MediatR;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;

namespace PokeGame.Domain.Moves;

public class Move : AggregateRoot
{
  public const int PowerMaximumValue = 250;
  public const int PowerPointsMaximumValue = 40;
  public const int StageMaximumValue = 6;
  public const int StageMinimumValue = -6;

  private UpdatedEvent _updatedEvent = new();

  public new MoveId Id => new(base.Id);

  public PokemonType Type { get; private set; }
  public MoveCategory Category { get; private set; }

  private UniqueName? _uniqueName = null;
  public UniqueName UniqueName
  {
    get => _uniqueName ?? throw new InvalidOperationException($"The {nameof(UniqueName)} has not been initialized yet.");
    set
    {
      if (_uniqueName != value)
      {
        _uniqueName = value;
        _updatedEvent.UniqueName = value;
      }
    }
  }
  private DisplayName? _displayName = null;
  public DisplayName? DisplayName
  {
    get => _displayName;
    set
    {
      if (_displayName != value)
      {
        _displayName = value;
        _updatedEvent.DisplayName = new Change<DisplayName>(value);
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
      else if (Category == MoveCategory.Status && value.HasValue)
      {
        throw new StatusMoveCannotHavePowerException(this, value.Value, nameof(Power));
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

  public Move(PokemonType type, MoveCategory category, UniqueName uniqueName, UserId userId, MoveId? id = null) : base((id ?? MoveId.NewId()).AggregateId)
  {
    if (!Enum.IsDefined(type))
    {
      throw new ArgumentOutOfRangeException(nameof(type));
    }
    if (!Enum.IsDefined(category))
    {
      throw new ArgumentOutOfRangeException(nameof(category));
    }

    Raise(new CreatedEvent(type, category, uniqueName), userId.ActorId);
  }
  protected virtual void Apply(CreatedEvent @event)
  {
    Type = @event.Type;
    Category = @event.Category;

    _uniqueName = @event.UniqueName;
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

    if (stages == 0)
    {
      if (_statisticChanges.Remove(statistic))
      {
        _updatedEvent.StatisticChanges[statistic] = stages;
      }
    }
    else if (!_statisticChanges.TryGetValue(statistic, out int existingStages) || existingStages != stages)
    {
      _statisticChanges[statistic] = stages;
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
    if (@event.UniqueName != null)
    {
      _uniqueName = @event.UniqueName;
    }
    if (@event.DisplayName != null)
    {
      _displayName = @event.DisplayName.Value;
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

  public override string ToString() => $"{DisplayName?.Value ?? UniqueName.Value} | {base.ToString()}";

  public class CreatedEvent : DomainEvent, INotification
  {
    public PokemonType Type { get; }
    public MoveCategory Category { get; }

    public UniqueName UniqueName { get; }

    public CreatedEvent(PokemonType type, MoveCategory category, UniqueName uniqueName)
    {
      Type = type;
      Category = category;

      UniqueName = uniqueName;
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
    public UniqueName? UniqueName { get; set; }
    public Change<DisplayName>? DisplayName { get; set; }
    public Change<Description>? Description { get; set; }

    public Change<int?>? Accuracy { get; set; }
    public Change<int?>? Power { get; set; }
    public int? PowerPoints { get; set; }

    public Dictionary<BattleStatistic, int> StatisticChanges { get; set; } = [];
    public Change<InflictedCondition>? Status { get; set; }
    public Dictionary<VolatileCondition, ActionKind> VolatileConditions { get; set; } = [];

    public Change<Url>? Link { get; set; }
    public Change<Notes>? Notes { get; set; }

    public bool HasChanges => UniqueName != null || DisplayName != null || Description != null
      || Accuracy != null || Power != null || PowerPoints != null
      || StatisticChanges.Count > 0 || Status != null || VolatileConditions.Count > 0
      || Link != null || Notes != null;
  }
}
