using PokeGame.Core.Abilities;
using PokeGame.Core.Species.Events;
using PokeGame.Core.Species.Payloads;

namespace PokeGame.Core.Species
{
  public class Species : Aggregate
  {
    public Species(CreateSpeciesPayload payload, Guid userId, Ability? ability = null)
    {
      ApplyChange(new CreatedEvent(payload, userId));

      Ability = ability;
      AbilitySid = ability?.Sid;
    }
    private Species()
    {
    }

    public int Number { get; private set; }

    public PokemonType PrimaryType { get; private set; }
    public PokemonType? SecondaryType { get; private set; }

    public Ability? Ability { get; private set; }
    public int? AbilitySid { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Category { get; private set; }
    public string? Description { get; private set; }

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateSpeciesPayload payload, Guid userId, Ability? ability = null)
    {
      ApplyChange(new UpdatedEvent(payload, userId));

      Ability = ability;
      AbilitySid = ability?.Sid;
    }

    protected virtual void Apply(CreatedEvent @event)
    {
      Number = @event.Payload.Number;

      PrimaryType = @event.Payload.PrimaryType;
      SecondaryType = @event.Payload.SecondaryType;

      Apply(@event.Payload);
    }
    protected virtual void Apply(DeletedEvent @event)
    {
    }
    protected virtual void Apply(UpdatedEvent @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SaveSpeciesPayload payload)
    {
      Name = payload.Name.Trim();
      Category = payload.Category?.CleanTrim();
      Description = payload.Description?.CleanTrim();

      Notes = payload.Notes?.CleanTrim();
      Reference = payload.Reference;
    }

    public override string ToString() => $"No. {Number:d3} {Name} | {base.ToString()}";
  }
}
