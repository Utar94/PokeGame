using Logitar.EventSourcing;
using MediatR;
using PokeGame.Contracts;
using PokeGame.Contracts.Species;

namespace PokeGame.Domain.Species;

public class PokemonSpecies : AggregateRoot
{
  private UpdatedEvent _updatedEvent = new();

  public new SpeciesId Id => new(base.Id);

  public int Number { get; private set; } // TODO(fpion): can we change Number?
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
  private PokemonCategory? _category = null;
  public PokemonCategory? Category
  {
    get => _category;
    set
    {
      if (value.HasValue && !Enum.IsDefined(value.Value))
      {
        throw new ArgumentOutOfRangeException(nameof(Category));
      }

      if (_category != value)
      {
        _category = value;
        _updatedEvent.Category = new Change<PokemonCategory?>(value);
      }
    }
  }

  private int _baseHappiness = 0;
  public int BaseHappiness
  {
    get => _baseHappiness;
    set
    {
      if (value < 0 || value > byte.MaxValue)
      {
        throw new ArgumentOutOfRangeException(nameof(BaseHappiness));
      }

      if (_baseHappiness != value)
      {
        _baseHappiness = value;
        _updatedEvent.BaseHappiness = value;
      }
    }
  }
  private int _captureRate = 0;
  public int CaptureRate
  {
    get => _captureRate;
    set
    {
      if (value < 0 || value > byte.MaxValue)
      {
        throw new ArgumentOutOfRangeException(nameof(CaptureRate));
      }

      if (_captureRate != value)
      {
        _captureRate = value;
        _updatedEvent.CaptureRate = value;
      }
    }
  }
  public LevelingRate LevelingRate { get; private set; } // TODO(fpion): can we change LevelingRate?

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

  // TODO(fpion): PokedexNumbers

  public PokemonSpecies() : base()
  {
  }

  public PokemonSpecies(int number, UniqueName uniqueName, LevelingRate levelingRate, UserId userId, SpeciesId? id = null)
    : base((id ?? SpeciesId.NewId()).AggregateId)
  {
    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(number, nameof(number));

    if (!Enum.IsDefined(levelingRate))
    {
      throw new ArgumentOutOfRangeException(nameof(levelingRate));
    }

    Raise(new CreatedEvent(number, uniqueName, levelingRate), userId.ActorId);
  }
  protected virtual void Apply(CreatedEvent @event)
  {
    Number = @event.Number;
    _uniqueName = @event.UniqueName;

    LevelingRate = @event.LevelingRate;
  }

  public void Delete(UserId userId)
  {
    if (!IsDeleted)
    {
      Raise(new DeletedEvent(), userId.ActorId);
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
    if (@event.Category != null)
    {
      _category = @event.Category.Value;
    }

    if (@event.BaseHappiness.HasValue)
    {
      _baseHappiness = @event.BaseHappiness.Value;
    }
    if (@event.CaptureRate.HasValue)
    {
      _captureRate = @event.CaptureRate.Value;
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
    public int Number { get; }
    public UniqueName UniqueName { get; }

    public LevelingRate LevelingRate { get; }

    public CreatedEvent(int number, UniqueName uniqueName, LevelingRate levelingRate)
    {
      Number = number;
      UniqueName = uniqueName;

      LevelingRate = levelingRate;
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
    public Change<PokemonCategory?>? Category { get; set; }

    public int? BaseHappiness { get; set; }
    public int? CaptureRate { get; set; }

    public Change<Url>? Link { get; set; }
    public Change<Notes>? Notes { get; set; }

    public bool HasChanges => UniqueName != null || DisplayName != null || Category != null
      || BaseHappiness.HasValue || CaptureRate.HasValue
      || Link != null || Notes != null;
  }
}
