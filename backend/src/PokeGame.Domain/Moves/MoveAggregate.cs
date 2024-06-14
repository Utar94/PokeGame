using FluentValidation;
using FluentValidation.Results;
using Logitar.EventSourcing;
using Logitar.Identity.Contracts;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Moves.Events;
using PokeGame.Domain.Moves.Validators;

namespace PokeGame.Domain.Moves;

public class MoveAggregate : AggregateRoot
{
  public static readonly IUniqueNameSettings UniqueNameSettings = new ReadOnlyUniqueNameSettings("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_");

  private MoveUpdatedEvent _updatedEvent = new();

  public new MoveId Id => new(base.Id);

  public PokemonType Type { get; private set; }
  public MoveCategory Category { get; private set; }

  private UniqueNameUnit? _uniqueName = null;
  public UniqueNameUnit UniqueName
  {
    get => _uniqueName ?? throw new InvalidOperationException($"The {nameof(UniqueName)} has not been initialized yet.");
    set
    {
      if (value != _uniqueName)
      {
        _uniqueName = value;
        _updatedEvent.UniqueName = value;
      }
    }
  }
  private DisplayNameUnit? _displayName = null;
  public DisplayNameUnit? DisplayName
  {
    get => _displayName;
    set
    {
      if (value != _displayName)
      {
        _displayName = value;
        _updatedEvent.DisplayName = new Modification<DisplayNameUnit>(value);
      }
    }
  }
  private DescriptionUnit? _description = null;
  public DescriptionUnit? Description
  {
    get => _description;
    set
    {
      if (value != _description)
      {
        _description = value;
        _updatedEvent.Description = new Modification<DescriptionUnit>(value);
      }
    }
  }

  private readonly AccuracyValidator _accuracyValidator = new();
  private int? _accuracy = null;
  /// <summary>
  /// Gets or sets the move accuracy, which is a percentage value between 1 and 100.
  /// <br />When it is null, the move will never fail.
  /// </summary>
  public int? Accuracy
  {
    get => _accuracy;
    set
    {
      if (value != _accuracy)
      {
        if (value.HasValue)
        {
          _accuracyValidator.ValidateAndThrow(value.Value);
        }

        _accuracy = value;
        _updatedEvent.Accuracy = new Modification<int?>(value);
      }
    }
  }
  private readonly PowerValidator _powerValidator = new();
  private int? _power = null;
  /// <summary>
  /// Gets or sets the move power, which is an integer between 1 and 250.
  /// <br />When it is null, the move does not inflict damage, or inflicts damage while bypassing the standard calculation.
  /// <br />Moves belonging to the <see cref="MoveCategory.Status"/> cannot have power.
  /// </summary>
  public int? Power
  {
    get => _power;
    set
    {
      if (value != _power)
      {
        if (value.HasValue)
        {
          if (Category == MoveCategory.Status)
          {
            ValidationFailure error = new(nameof(Power), $"'{{PropertyName}}' must be null when the move category is '{MoveCategory.Status}'.", value)
            {
              ErrorCode = "InvalidMovePower"
            };
            throw new ValidationException([error]);
          }

          _powerValidator.ValidateAndThrow(value.Value);
        }

        _power = value;
        _updatedEvent.Power = new Modification<int?>(value);
      }
    }
  }
  private readonly PowerPointsValidator _powerPointsValidator = new();
  private int _powerPoints = 0;
  /// <summary>
  /// Gets or sets the move power points, which is an integer between 1 and 40.
  /// </summary>
  public int PowerPoints
  {
    get => _powerPoints;
    set
    {
      if (value != _powerPoints)
      {
        _powerPointsValidator.ValidateAndThrow(value);

        _powerPoints = value;
        _updatedEvent.PowerPoints = value;
      }
    }
  }

  private readonly Dictionary<PokemonStatistic, int> _statisticChanges = [];
  public IReadOnlyDictionary<PokemonStatistic, int> StatisticChanges => _statisticChanges.AsReadOnly();
  public void RemoveStatisticChange(PokemonStatistic statistic) => SetStatisticChange(statistic, stages: 0);
  public void SetStatisticChange(PokemonStatistic statistic, int stages)
  {
    if (!Enum.IsDefined(statistic))
    {
      throw new ArgumentOutOfRangeException(nameof(statistic));
    }

    List<ValidationFailure> errors = new(capacity: 2);
    if (statistic == PokemonStatistic.HP)
    {
      errors.Add(new ValidationFailure("Statistic", $"'{{PropertyName}}' must not be '{PokemonStatistic.HP}'.", statistic)
      {
        ErrorCode = "InvalidStatisticChange"
      });
    }
    if (stages < -6 || stages > 6)
    {
      errors.Add(new ValidationFailure("Stages", "'{PropertyName}' must be between -6 and 6.", stages)
      {
        ErrorCode = "InvalidStatisticChange"
      });
    }
    if (errors.Count > 0)
    {
      throw new ValidationException(errors);
    }

    if (stages == 0)
    {
      if (_statisticChanges.Remove(statistic))
      {
        _updatedEvent.StatisticChanges[statistic.ToString()] = 0;
      }
    }
    else if (!_statisticChanges.TryGetValue(statistic, out int existingStages) || stages != existingStages)
    {
      _statisticChanges[statistic] = stages;
      _updatedEvent.StatisticChanges[statistic.ToString()] = stages;
    }
  }

  private readonly Dictionary<StatusCondition, int> _statusConditions = [];
  public IReadOnlyDictionary<StatusCondition, int> StatusConditions => _statusConditions.AsReadOnly();
  public void RemoveStatusCondition(StatusCondition statusCondition) => SetStatusCondition(statusCondition, chance: 0);
  public void SetStatusCondition(StatusCondition statusCondition, int chance)
  {
    if (string.IsNullOrEmpty(statusCondition.Value))
    {
      throw new ArgumentOutOfRangeException(nameof(statusCondition));
    }

    List<ValidationFailure> errors = new(capacity: 1);
    if (chance < 0)
    {
      errors.Add(new ValidationFailure("Chance", "'{PropertyName}' must be greater than or equal to 0.", chance)
      {
        ErrorCode = "InvalidStatusCondition"
      });
    }
    else if (chance > 100)
    {
      errors.Add(new ValidationFailure("Chance", "'{PropertyName}' must be less than or equal to 100.", chance)
      {
        ErrorCode = "InvalidStatusCondition"
      });
    }
    if (errors.Count > 0)
    {
      throw new ValidationException(errors);
    }

    if (chance == 0)
    {
      if (_statusConditions.Remove(statusCondition))
      {
        _updatedEvent.StatusConditions[statusCondition.Value] = 0;
      }
    }
    else if (!_statusConditions.TryGetValue(statusCondition, out int existingChance) || chance != existingChance)
    {
      _statusConditions[statusCondition] = chance;
      _updatedEvent.StatusConditions[statusCondition.Value] = chance;
    }
  }

  private UrlUnit? _reference = null;
  public UrlUnit? Reference
  {
    get => _reference;
    set
    {
      if (value != _reference)
      {
        _reference = value;
        _updatedEvent.Reference = new Modification<UrlUnit>(value);
      }
    }
  }
  private NotesUnit? _notes = null;
  public NotesUnit? Notes
  {
    get => _notes;
    set
    {
      if (value != _notes)
      {
        _notes = value;
        _updatedEvent.Notes = new Modification<NotesUnit>(value);
      }
    }
  }

  public MoveAggregate() : base()
  {
  }

  public MoveAggregate(PokemonType type, MoveCategory category, UniqueNameUnit uniqueName, ActorId actorId = default, MoveId? id = null)
    : base((id ?? MoveId.NewId()).AggregateId)
  {
    if (!Enum.IsDefined(type))
    {
      throw new ArgumentOutOfRangeException(nameof(type));
    }
    if (!Enum.IsDefined(category))
    {
      throw new ArgumentOutOfRangeException(nameof(category));
    }

    Raise(new MoveCreatedEvent(type, category, uniqueName, powerPoints: 1), actorId);
  }
  protected virtual void Apply(MoveCreatedEvent @event)
  {
    Type = @event.Type;
    Category = @event.Category;

    _uniqueName = @event.UniqueName;

    _powerPoints = @event.PowerPoints;
  }

  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      Raise(new MoveDeletedEvent(), actorId);
    }
  }

  public void Update(ActorId actorId = default)
  {
    if (_updatedEvent.HasChanges)
    {
      Raise(_updatedEvent, actorId, DateTime.Now);
      _updatedEvent = new();
    }
  }
  protected virtual void Apply(MoveUpdatedEvent @event)
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

    foreach (KeyValuePair<string, int> statisticChange in @event.StatisticChanges)
    {
      PokemonStatistic key = Enum.Parse<PokemonStatistic>(statisticChange.Key);
      if (statisticChange.Value == 0)
      {
        _statisticChanges.Remove(key);
      }
      else
      {
        _statisticChanges[key] = statisticChange.Value;
      }
    }
    foreach (KeyValuePair<string, int> statusCondition in @event.StatusConditions)
    {
      StatusCondition key = new(statusCondition.Key);
      if (statusCondition.Value == 0)
      {
        _statusConditions.Remove(key);
      }
      else
      {
        _statusConditions[key] = statusCondition.Value;
      }
    }

    if (@event.Reference != null)
    {
      _reference = @event.Reference.Value;
    }
    if (@event.Notes != null)
    {
      _notes = @event.Notes.Value;
    }
  }

  public override string ToString() => $"{DisplayName?.Value ?? UniqueName.Value} | {base.ToString()}";
}
