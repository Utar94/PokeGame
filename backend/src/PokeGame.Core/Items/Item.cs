using PokeGame.Core.Items.Events;
using PokeGame.Core.Items.Payloads;

namespace PokeGame.Core.Items
{
  public class Item : Aggregate
  {
    public Item(CreateItemPayload payload, Guid userId)
    {
      ApplyChange(new CreatedEvent(payload, userId));
    }
    private Item()
    {
    }

    public Category Category { get; private set; }

    public int? Price { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateItemPayload payload, Guid userId) => ApplyChange(new UpdatedEvent(payload, userId));

    protected virtual void Apply(CreatedEvent @event)
    {
      Category = @event.Payload.Category;

      Apply(@event.Payload);
    }
    protected virtual void Apply(DeletedEvent @event)
    {
    }
    protected virtual void Apply(UpdatedEvent @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SaveItemPayload payload)
    {
      Price = payload.Price;

      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      Notes = payload.Notes?.CleanTrim();
      Reference = payload.Reference;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
