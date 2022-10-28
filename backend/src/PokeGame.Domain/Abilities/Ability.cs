using PokeGame.Domain.Abilities.Events;
using PokeGame.Domain.Abilities.Payloads;

namespace PokeGame.Domain.Abilities
{
  public class Ability : Aggregate
  {
    public Ability(CreateAbilityPayload payload)
    {
      ApplyChange(new AbilityCreated(payload));
    }
    private Ability()
    {
    }

    public AbilityKind? Kind { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public void Delete() => ApplyChange(new AbilityDeleted());
    public void Update(UpdateAbilityPayload payload) => ApplyChange(new AbilityUpdated(payload));

    protected virtual void Apply(AbilityCreated @event)
    {
      Apply(@event.Payload);
    }
    protected virtual void Apply(AbilityDeleted @event)
    {
      Delete(@event);
    }
    protected virtual void Apply(AbilityUpdated @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SaveAbilityPayload payload)
    {
      Kind = payload.Kind;

      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      Notes = payload.Notes?.CleanTrim();
      Reference = payload.Reference;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
