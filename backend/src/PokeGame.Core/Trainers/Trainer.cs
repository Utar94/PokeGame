using PokeGame.Core.Inventories;
using PokeGame.Core.Items;
using PokeGame.Core.Trainers.Events;
using PokeGame.Core.Trainers.Payloads;

namespace PokeGame.Core.Trainers
{
  public class Trainer : Aggregate
  {
    public Trainer(CreateTrainerPayload payload, Guid userId)
    {
      ApplyChange(new CreatedEvent(payload, userId));
    }
    private Trainer()
    {
    }

    public Guid? UserId { get; private set; }

    public Region Region { get; private set; }
    public int Number { get; private set; }
    public byte Checksum
    {
      get => string.Concat((char)Region, Number).Checksum();
      private set { /* Empty setter for EntityFrameworkCore */ }
    }

    public int Money { get; private set; }

    public TrainerGender Gender { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public List<Inventory> Inventory { get; private set; } = new();

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateTrainerPayload payload, Guid userId) => ApplyChange(new UpdatedEvent(payload, userId));

    public void AddItem(Item item, ushort quantity, Guid userId)
    {
      ArgumentNullException.ThrowIfNull(item);

      ApplyChange(new AddedItemEvent(item.Id, quantity, userId));
    }
    public void BuyItem(Item item, ushort quantity, Guid userId)
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

      ApplyChange(new BoughtItemEvent(item.Id, item.Price.Value, quantity, userId));
    }
    public void RemoveItem(Item item, ushort quantity, Guid userId)
    {
      ArgumentNullException.ThrowIfNull(item);

      Inventory inventory = Inventory.SingleOrDefault(x => x.Item?.Equals(item) == true)
        ?? throw new InsufficientQuantityException(item, quantity);

      int missingQuantity = inventory.Quantity - quantity;
      if (missingQuantity < 0)
      {
        throw new InsufficientQuantityException(item, -missingQuantity);
      }

      ApplyChange(new RemovedItemEvent(item.Id, quantity, userId));
    }
    public void SellItem(Item item, ushort quantity, Guid userId)
    {
      ArgumentNullException.ThrowIfNull(item);

      if (!item.Price.HasValue)
      {
        throw new ItemPriceRequiredException(item);
      }

      Inventory inventory = Inventory.SingleOrDefault(x => x.Item?.Equals(item) == true)
        ?? throw new InsufficientQuantityException(item, quantity);

      int missingQuantity = inventory.Quantity - quantity;
      if (missingQuantity < 0)
      {
        throw new InsufficientQuantityException(item, -missingQuantity);
      }

      ApplyChange(new SoldItemEvent(item.Id, item.Price.Value, quantity, userId));
    }

    protected virtual void Apply(AddedItemEvent @event)
    {
    }
    protected virtual void Apply(BoughtItemEvent @event)
    {
      Money -= @event.ItemPrice * @event.Quantity;
    }
    protected virtual void Apply(CreatedEvent @event)
    {
      Region = @event.Payload.Region;
      Number = @event.Payload.Number;

      Gender = @event.Payload.Gender;

      Apply(@event.Payload);
    }
    protected virtual void Apply(DeletedEvent @event)
    {
    }
    protected virtual void Apply(RemovedItemEvent @event)
    {
    }
    protected virtual void Apply(SoldItemEvent @event)
    {
      Money += @event.ItemPrice * @event.Quantity;
    }
    protected virtual void Apply(UpdatedEvent @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SaveTrainerPayload payload)
    {
      UserId = payload.UserId;

      Money = payload.Money;

      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      Notes = payload.Notes?.CleanTrim();
      Reference = payload.Reference;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
