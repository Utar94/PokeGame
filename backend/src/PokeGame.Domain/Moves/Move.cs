using Logitar;
using Logitar.EventSourcing;
using PokeGame.Domain.Moves.Events;

namespace PokeGame.Domain.Moves;

public class Move : AggregateRoot
{
  private MoveUpdated _updated = new();

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
        _updated.UniqueName = value;
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
        _updated.DisplayName = new Change<DisplayName>(value);
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
        _updated.Description = new Change<Description>(value);
      }
    }
  }

  private Accuracy? _accuracy = null;
  public Accuracy? Accuracy
  {
    get => _accuracy;
    set
    {
      if (_accuracy != value)
      {
        _accuracy = value;
        _updated.Accuracy = new Change<Accuracy>(value);
      }
    }
  }
  private Power? _power = null;
  public Power? Power
  {
    get => _power;
    set
    {
      if (Category == MoveCategory.Status && value != null)
      {
        throw new StatusMoveCannotHavePowerException(this);
      }

      if (_power != value)
      {
        _power = value;
        _updated.Power = new Change<Power>(value);
      }
    }
  }
  private PowerPoints? _powerPoints = null;
  public PowerPoints PowerPoints
  {
    get => _powerPoints ?? throw new InvalidOperationException($"The {nameof(UniqueName)} has not been initialized yet.");
    set
    {
      if (_powerPoints != value)
      {
        _powerPoints = value;
        _updated.PowerPoints = value;
      }
    }
  }

  private InflictedStatus? _inflictedStatus = null;
  public InflictedStatus? InflictedStatus
  {
    get => _inflictedStatus;
    set
    {
      if (_inflictedStatus != value)
      {
        _inflictedStatus = value;
        _updated.InflictedStatus = new Change<InflictedStatus>(value);
      }
    }
  }
  private readonly Dictionary<PokemonStatistic, int> _statisticChanges = [];
  public IReadOnlyDictionary<PokemonStatistic, int> StatisticChanges => _statisticChanges.AsReadOnly();
  private readonly HashSet<VolatileCondition> _volatileConditions = [];
  public IReadOnlyCollection<VolatileCondition> VolatileConditions => [.. _volatileConditions];

  private Url? _link = null;
  public Url? Link
  {
    get => _link;
    set
    {
      if (_link != value)
      {
        _link = value;
        _updated.Link = new Change<Url>(value);
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
        _updated.Notes = new Change<Notes>(value);
      }
    }
  }

  public Move() : base()
  {
  }

  public Move(PokemonType type, MoveCategory category, UniqueName uniqueName, PowerPoints powerPoints, UserId userId, MoveId? id = null)
    : base((id ?? MoveId.NewId()).StreamId)
  {
    if (!Enum.IsDefined(type))
    {
      throw new ArgumentOutOfRangeException(nameof(type));
    }
    if (!Enum.IsDefined(category))
    {
      throw new ArgumentOutOfRangeException(nameof(category));
    }

    Raise(new MoveCreated(type, category, uniqueName, powerPoints), userId.ActorId);
  }
  protected virtual void Handle(MoveCreated @event)
  {
    Type = @event.Type;
    Category = @event.Category;

    _uniqueName = @event.UniqueName;

    _powerPoints = @event.PowerPoints;
  }

  public void Delete(UserId userId)
  {
    if (!IsDeleted)
    {
      Raise(new MoveDeleted(), userId.ActorId);
    }
  }

  public void SetStatisticChange(PokemonStatistic statistic, int stages)
  {
    if (!Enum.IsDefined(statistic))
    {
      throw new ArgumentOutOfRangeException(nameof(statistic));
    }
    if (statistic == PokemonStatistic.HP)
    {
      throw new ArgumentException($"The statistic cannot be {nameof(PokemonStatistic.HP)}.", nameof(statistic));
    }
    if (stages < -6 || stages > 6)
    {
      throw new ArgumentOutOfRangeException(nameof(stages), "The stages must range between -6 and 6.");
    }

    _ = _statisticChanges.TryGetValue(statistic, out int existingStages);
    if (existingStages != stages)
    {
      _updated.StatisticChanges[statistic] = stages;
    }
  }

  public bool HasVolatileCondition(VolatileCondition volatileCondition) => _volatileConditions.Contains(volatileCondition);
  public void SetVolatileConditions(IEnumerable<VolatileCondition> volatileConditions)
  {
    IEnumerable<VolatileCondition> distinct = volatileConditions.Distinct();
    if (!_volatileConditions.SequenceEqual(distinct))
    {
      _volatileConditions.Clear();
      _volatileConditions.AddRange(distinct);
      _updated.VolatileConditions = distinct.ToArray();
    }
  }

  public void Update(UserId userId)
  {
    if (_updated.HasChanges)
    {
      Raise(_updated, userId.ActorId, DateTime.Now);
      _updated = new();
    }
  }
  protected virtual void Handle(MoveUpdated @event)
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
      _powerPoints = @event.PowerPoints;
    }

    if (@event.InflictedStatus != null)
    {
      _inflictedStatus = @event.InflictedStatus.Value;
    }
    foreach (KeyValuePair<PokemonStatistic, int> statisticChange in @event.StatisticChanges)
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
    if (@event.VolatileConditions != null)
    {
      _volatileConditions.Clear();
      _volatileConditions.AddRange(@event.VolatileConditions);
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
}
