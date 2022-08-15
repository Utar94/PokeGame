using PokeGame.Core.Abilities.Events;
using PokeGame.Core.Abilities.Payloads;

namespace PokeGame.Core.Abilities
{
  public class Ability : Aggregate
  {
    public Ability(CreateAbilityPayload payload, Guid userId)
    {
      ApplyChange(new CreatedEvent(payload, userId));
    }
    private Ability()
    {
    }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public List<Species.Species> Species { get; private set; } = new();

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateAbilityPayload payload, Guid userId) => ApplyChange(new UpdatedEvent(payload, userId));

    protected virtual void Apply(CreatedEvent @event)
    {
      Apply(@event.Payload);
    }
    protected virtual void Apply(DeletedEvent @event)
    {
    }
    protected virtual void Apply(UpdatedEvent @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SaveAbilityPayload payload)
    {
      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      Notes = payload.Notes?.CleanTrim();
      Reference = payload.Reference;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
