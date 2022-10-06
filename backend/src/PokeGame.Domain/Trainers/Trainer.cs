using PokeGame.Domain.Items;
using PokeGame.Domain.Trainers.Events;
using PokeGame.Domain.Trainers.Payloads;

namespace PokeGame.Domain.Trainers
{
  public class Trainer : Aggregate
  {
    public Trainer(CreateTrainerPayload payload)
    {
      ApplyChange(new TrainerCreated(payload));
    }
    private Trainer()
    {
    }

    public Guid? UserId { get; private set; }

    public Region Region { get; private set; }
    public int Number { get; private set; }
    public byte Checksum => string.Concat((char)Region, Number).Checksum();

    public int Money { get; private set; }
    public int PlayTime { get; private set; }

    public TrainerGender Gender { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public string? Notes { get; private set; }
    public string? Picture { get; private set; }
    public string? Reference { get; private set; }

    public Dictionary<Guid, int> Inventory { get; private set; } = new();
    public Dictionary<Guid, PokedexEntry> Pokedex { get; private set; } = new();

    public void Delete() => ApplyChange(new TrainerDeleted());
    public void Update(UpdateTrainerPayload payload) => ApplyChange(new TrainerUpdated(payload));

    public void AddItem(Item item, ushort quantity)
    {
      ArgumentNullException.ThrowIfNull(item);

      ApplyChange(new AddedItem(item.Id, quantity));
    }
    public void BuyItem(Item item, ushort quantity)
    {
      ArgumentNullException.ThrowIfNull(item);

      if (!item.Price.HasValue)
      {
        throw new ItemPriceRequiredException(item);
      }

      int missingAmount = Money - (item.Price.Value * quantity);
      if (missingAmount < 0)
      {
        throw new InsufficientMoneyException(-missingAmount);
      }

      ApplyChange(new BoughtItem(item.Id, item.Price.Value, quantity));
    }
    public void RemoveItem(Item item, ushort quantity)
    {
      ArgumentNullException.ThrowIfNull(item);

      int inventory = Inventory.ContainsKey(item.Id) ? Inventory[item.Id] : 0;
      int missingQuantity = inventory - quantity;
      if (missingQuantity < 0)
      {
        throw new InsufficientQuantityException(item, -missingQuantity);
      }

      ApplyChange(new RemovedItem(item.Id, quantity));
    }
    public void SellItem(Item item, ushort quantity)
    {
      ArgumentNullException.ThrowIfNull(item);

      if (!item.Price.HasValue)
      {
        throw new ItemPriceRequiredException(item);
      }

      int inventory = Inventory.ContainsKey(item.Id) ? Inventory[item.Id] : 0;
      int missingQuantity = inventory - quantity;
      if (missingQuantity < 0)
      {
        throw new InsufficientQuantityException(item, -missingQuantity);
      }

      ApplyChange(new SoldItem(item.Id, item.Price.Value, quantity));
    }

    public void RemovePokedex(Guid speciesId)
    {
      if (!Pokedex.ContainsKey(speciesId))
      {
        throw new PokedexEntryNotFoundException(this, speciesId);
      }

      ApplyChange(new RemovedPokedex(speciesId));
    }
    public void SavePokedex(Guid speciesId, bool hasCaught) => ApplyChange(new SavedPokedex(speciesId, hasCaught));

    protected virtual void Apply(AddedItem @event)
    {
      AddItem(@event.ItemId, @event.Quantity);
    }
    protected virtual void Apply(BoughtItem @event)
    {
      Money -= @event.ItemPrice * @event.Quantity;

      AddItem(@event.ItemId, @event.Quantity);
    }
    protected virtual void Apply(RemovedPokedex @event)
    {
      Pokedex.Remove(@event.SpeciesId);
    }
    protected virtual void Apply(SavedPokedex @event)
    {
      Pokedex[@event.SpeciesId] = new(@event.HasCaught, @event.OccurredAt);
    }
    protected virtual void Apply(TrainerCreated @event)
    {
      Region = @event.Payload.Region;
      Number = @event.Payload.Number;

      Gender = @event.Payload.Gender;

      Apply(@event.Payload);
    }
    protected virtual void Apply(TrainerDeleted @event)
    {
      Delete(@event);
    }
    protected virtual void Apply(RemovedItem @event)
    {
      RemoveItem(@event.ItemId, @event.Quantity);
    }
    protected virtual void Apply(SoldItem @event)
    {
      Money += @event.ItemPrice * @event.Quantity;

      RemoveItem(@event.ItemId, @event.Quantity);
    }
    protected virtual void Apply(TrainerUpdated @event)
    {
      Apply(@event.Payload);
    }

    private void AddItem(Guid itemId, ushort quantity)
    {
      if (Inventory.ContainsKey(itemId))
      {
        Inventory[itemId] += quantity;
      }
      else
      {
        Inventory[itemId] = quantity;
      }
    }
    private void RemoveItem(Guid itemId, ushort quantity)
    {
      if (Inventory.TryGetValue(itemId, out int inventory))
      {
        if (inventory > quantity)
        {
          Inventory[itemId] -= quantity;
        }
        else
        {
          Inventory.Remove(itemId);
        }
      }
    }

    private void Apply(SaveTrainerPayload payload)
    {
      UserId = payload.UserId;

      Money = payload.Money;
      PlayTime = payload.PlayTime;

      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      Notes = payload.Notes?.CleanTrim();
      Picture = payload.Picture;
      Reference = payload.Reference;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
