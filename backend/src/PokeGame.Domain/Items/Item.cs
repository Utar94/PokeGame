using PokeGame.Domain.Items.Events;
using PokeGame.Domain.Items.Payloads;

namespace PokeGame.Domain.Items
{
  public class Item : Aggregate
  {
    public Item(CreateItemPayload payload)
    {
      ApplyChange(new ItemCreated(payload));
    }
    private Item()
    {
    }

    public ItemCategory Category { get; private set; }
    public ItemType? Type { get; private set; }
    public double? DefaultModifier { get; private set; }

    public int? Price { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public string? Notes { get; private set; }
    public string? Picture { get; private set; }
    public string? Reference { get; private set; }

    public void Delete() => ApplyChange(new ItemDeleted());
    public void Update(UpdateItemPayload payload) => ApplyChange(new ItemUpdated(payload));

    protected virtual void Apply(ItemCreated @event)
    {
      Category = @event.Payload.Category;

      Apply(@event.Payload);
    }
    protected virtual void Apply(ItemDeleted @event)
    {
      Delete(@event);
    }
    protected virtual void Apply(ItemUpdated @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SaveItemPayload payload)
    {
      Type = payload.Type;
      DefaultModifier = payload.DefaultModifier;

      Price = payload.Price;

      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      Notes = payload.Notes?.CleanTrim();
      Picture = payload.Picture;
      Reference = payload.Reference;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
