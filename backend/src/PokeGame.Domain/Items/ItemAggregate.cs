using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Contracts;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using PokeGame.Contracts.Items;
using PokeGame.Domain.Items.Events;
using PokeGame.Domain.Items.Properties;
using PokeGame.Domain.Items.Validators;

namespace PokeGame.Domain.Items;

public class ItemAggregate : AggregateRoot
{
  public static readonly IUniqueNameSettings UniqueNameSettings = new ReadOnlyUniqueNameSettings("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_");

  private ItemUpdatedEvent _updatedEvent = new();

  public new ItemId Id => new(base.Id);

  public ItemCategory Category { get; private set; }

  private readonly PriceValidator _priceValidator = new();
  private int? _price = null;
  public int? Price
  {
    get => _price;
    set
    {
      if (value != _price)
      {
        if (value.HasValue)
        {
          _priceValidator.ValidateAndThrow(value.Value);
        }

        _price = value;
        _updatedEvent.Price = new Modification<int?>(value);
      }
    }
  }

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
  private UrlUnit? _picture = null;
  public UrlUnit? Picture
  {
    get => _picture;
    set
    {
      if (value != _picture)
      {
        _picture = value;
        _updatedEvent.Picture = new Modification<UrlUnit>(value);
      }
    }
  }

  private ItemProperties? _properties = null;
  public ItemProperties Properties => _properties ?? throw new InvalidOperationException($"The {nameof(Properties)} has not been initialized yet.");

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

  public ItemAggregate() : base()
  {
  }

  public ItemAggregate(ItemCategory category, UniqueNameUnit uniqueName, ActorId actorId = default, ItemId? id = null)
    : base((id ?? ItemId.NewId()).AggregateId)
  {
    if (!Enum.IsDefined(category))
    {
      throw new ArgumentOutOfRangeException(nameof(category));
    }

    Raise(new ItemCreatedEvent(category, uniqueName), actorId);
  }
  protected virtual void Apply(ItemCreatedEvent @event)
  {
    Category = @event.Category;

    _uniqueName = @event.UniqueName;
  }

  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      Raise(new ItemDeletedEvent(), actorId);
    }
  }

  public void SetProperties(ReadOnlyMedicineProperties properties, ActorId actorId = default)
  {
    if (Category != ItemCategory.Medicine)
    {
      throw new ItemCategoryMismatchException(this, ItemCategory.Medicine);
    }
    else if (properties != _properties)
    {
      Raise(new MedicinePropertiesChangedEvent(properties), actorId);
    }
  }
  protected virtual void Apply(MedicinePropertiesChangedEvent @event)
  {
    _properties = @event.Properties;
  }

  public void SetProperties(ReadOnlyPokeBallProperties properties, ActorId actorId = default)
  {
    if (Category != ItemCategory.PokeBall)
    {
      throw new ItemCategoryMismatchException(this, ItemCategory.PokeBall);
    }
    else if (properties != _properties)
    {
      Raise(new PokeBallPropertiesChangedEvent(properties), actorId);
    }
  }
  protected virtual void Apply(PokeBallPropertiesChangedEvent @event)
  {
    _properties = @event.Properties;
  }

  public void Update(ActorId actorId = default)
  {
    if (_updatedEvent.HasChanges)
    {
      Raise(_updatedEvent, actorId, DateTime.Now);
      _updatedEvent = new();
    }
  }
  protected virtual void Apply(ItemUpdatedEvent @event)
  {
    if (@event.Price != null)
    {
      _price = @event.Price.Value;
    }

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
    if (@event.Picture != null)
    {
      _picture = @event.Picture.Value;
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
