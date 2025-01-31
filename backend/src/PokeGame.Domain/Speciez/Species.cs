using Logitar.EventSourcing;
using PokeGame.Domain.Regions;
using PokeGame.Domain.Speciez.Events;

namespace PokeGame.Domain.Speciez;

public class Species : AggregateRoot
{
  private SpeciesUpdated _updated = new();

  public new SpeciesId Id => new(base.Id);

  public int Number { get; private set; }
  public SpeciesCategory Category { get; private set; }

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

  private GrowthRate? _growthRate = null;
  public GrowthRate GrowthRate
  {
    get => _growthRate ?? throw new InvalidOperationException($"The {nameof(GrowthRate)} has not been initialized yet.");
    set
    {
      if (_growthRate != value)
      {
        _growthRate = value;
        _updated.GrowthRate = value;
      }
    }
  }
  private Friendship? _baseFriendship = null;
  public Friendship BaseFriendship
  {
    get => _baseFriendship ?? throw new InvalidOperationException($"The {nameof(BaseFriendship)} has not been initialized yet.");
    set
    {
      if (_baseFriendship != value)
      {
        _baseFriendship = value;
        _updated.BaseFriendship = value;
      }
    }
  }
  private CatchRate? _catchRate = null;
  public CatchRate CatchRate
  {
    get => _catchRate ?? throw new InvalidOperationException($"The {nameof(BaseFriendship)} has not been initialized yet.");
    set
    {
      if (_catchRate != value)
      {
        _catchRate = value;
        _updated.CatchRate = value;
      }
    }
  }

  private readonly Dictionary<RegionId, int> _regionalNumbers = [];
  public IReadOnlyDictionary<RegionId, int> RegionalNumbers => _regionalNumbers.AsReadOnly();

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

  public Species() : base()
  {
  }

  public Species(int number, SpeciesCategory category, UniqueName uniqueName, GrowthRate growthRate, Friendship baseFriendship, CatchRate catchRate, ActorId? actorId = null, SpeciesId? id = null)
    : base((id ?? SpeciesId.NewId()).StreamId)
  {
    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(number, nameof(number));
    if (!Enum.IsDefined(category))
    {
      throw new ArgumentOutOfRangeException(nameof(category));
    }

    Raise(new SpeciesCreated(number, category, uniqueName, growthRate, baseFriendship, catchRate), actorId);
  }
  protected virtual void Handle(SpeciesCreated @event)
  {
    Number = @event.Number;
    Category = @event.Category;

    _uniqueName = @event.UniqueName;

    _growthRate = @event.GrowthRate;
    _baseFriendship = @event.BaseFriendship;
    _catchRate = @event.CatchRate;
  }

  public void Delete(ActorId? actorId = null)
  {
    if (!IsDeleted)
    {
      Raise(new SpeciesDeleted(), actorId);
    }
  }

  public void SetRegionalNumber(Region region, int? number, ActorId? actorId = null)
  {
    if (number < 1)
    {
      throw new ArgumentOutOfRangeException(nameof(number), "The number must be greater than 0.");
    }

    int? existingNumber = _regionalNumbers.TryGetValue(region.Id, out int value) ? value : null;
    if (existingNumber != number)
    {
      Raise(new SpeciesRegionalNumberChanged(region.Id, number), actorId);
    }
  }
  protected virtual void Handle(SpeciesRegionalNumberChanged @event)
  {
    if (@event.Number.HasValue)
    {
      _regionalNumbers[@event.RegionId] = @event.Number.Value;
    }
    else
    {
      _regionalNumbers.Remove(@event.RegionId);
    }
  }

  public void Update(ActorId? actorId = null)
  {
    if (_updated.HasChanges)
    {
      Raise(_updated, actorId, DateTime.Now);
      _updated = new();
    }
  }
  protected virtual void Handle(SpeciesUpdated @event)
  {
    if (@event.UniqueName != null)
    {
      _uniqueName = @event.UniqueName;
    }
    if (@event.DisplayName != null)
    {
      _displayName = @event.DisplayName.Value;
    }

    if (@event.GrowthRate.HasValue)
    {
      _growthRate = @event.GrowthRate.Value;
    }
    if (@event.BaseFriendship != null)
    {
      _baseFriendship = @event.BaseFriendship;
    }
    if (@event.CatchRate != null)
    {
      _catchRate = @event.CatchRate;
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

  public override string ToString() => $"{DisplayName?.Value ?? UniqueName.Value} #{Number} | {base.ToString()}";
}
