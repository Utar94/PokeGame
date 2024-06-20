using Logitar;
using PokeGame.Contracts.Items;
using PokeGame.Contracts.Items.Properties;
using PokeGame.Domain.Items.Events;

namespace PokeGame.EntityFrameworkCore.Entities;

internal class ItemEntity : AggregateEntity
{
  public int ItemId { get; private set; }

  public ItemCategory Category { get; private set; }
  public int? Price { get; private set; }

  public string UniqueName { get; private set; } = string.Empty;
  public string UniqueNameNormalized
  {
    get => PokemonDb.Normalize(UniqueName);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }
  public string? Picture { get; private set; }

  public string? Reference { get; private set; }
  public string? Notes { get; private set; }

  public Dictionary<string, string> Properties { get; private set; } = [];
  public string? PropertiesSerialized
  {
    get => Properties.Count == 0 ? null : JsonSerializer.Serialize(Properties);
    private set
    {
      Properties.Clear();
      if (value != null)
      {
        Dictionary<string, string>? properties = JsonSerializer.Deserialize<Dictionary<string, string>>(value);
        if (properties != null)
        {
          Properties.AddRange(properties);
        }
      }
    }
  }

  public ItemEntity(ItemCreatedEvent @event) : base(@event)
  {
    Category = @event.Category;

    UniqueName = @event.UniqueName.Value;
  }

  private ItemEntity() : base()
  {
  }

  public void SetProperties(MedicinePropertiesChangedEvent @event)
  {
    Update(@event);

    Properties.Clear();
    SetProperty(nameof(IMedicineProperties.HitPointHealing), @event.Properties.HitPointHealing?.ToString());
    SetProperty(nameof(IMedicineProperties.IsHitPointPercentage), @event.Properties.IsHitPointPercentage.ToString());
    SetProperty(nameof(IMedicineProperties.DoesReviveFainted), @event.Properties.DoesReviveFainted.ToString());
    SetProperty(nameof(IMedicineProperties.RemoveStatusCondition), @event.Properties.RemoveStatusCondition?.ToString());
    SetProperty(nameof(IMedicineProperties.RemoveAllStatusConditions), @event.Properties.RemoveAllStatusConditions.ToString());
    SetProperty(nameof(IMedicineProperties.RestorePowerPoints), @event.Properties.RestorePowerPoints?.ToString());
    SetProperty(nameof(IMedicineProperties.IsPowerPointPercentage), @event.Properties.IsPowerPointPercentage.ToString());
    SetProperty(nameof(IMedicineProperties.RestoreAllMoves), @event.Properties.RestoreAllMoves.ToString());
    SetProperty(nameof(IMedicineProperties.FriendshipPenalty), @event.Properties.FriendshipPenalty?.ToString());
  }
  public void SetProperties(PokeBallPropertiesChangedEvent @event)
  {
    Update(@event);

    Properties.Clear();
    SetProperty(nameof(IPokeBallProperties.CatchRateModifier), @event.Properties.CatchRateModifier?.ToString());
  }
  private void SetProperty(string key, string? value)
  {
    if (value == null)
    {
      Properties.Remove(key);
    }
    else
    {
      Properties[key] = value.ToString();
    }
  }

  public void Update(ItemUpdatedEvent @event)
  {
    base.Update(@event);

    if (@event.Price != null)
    {
      Price = @event.Price.Value;
    }

    if (@event.UniqueName != null)
    {
      UniqueName = @event.UniqueName.Value;
    }
    if (@event.DisplayName != null)
    {
      DisplayName = @event.DisplayName.Value?.Value;
    }
    if (@event.Description != null)
    {
      Description = @event.Description.Value?.Value;
    }
    if (@event.Picture != null)
    {
      Picture = @event.Picture.Value?.Value;
    }

    if (@event.Reference != null)
    {
      Reference = @event.Reference.Value?.Value;
    }
    if (@event.Notes != null)
    {
      Notes = @event.Notes.Value?.Value;
    }
  }
}
