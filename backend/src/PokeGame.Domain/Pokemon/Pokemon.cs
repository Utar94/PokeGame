using PokeGame.Domain.Pokemon.Events;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon
{
  public class Pokemon : Aggregate
  {
    public Pokemon(CreatePokemonPayload payload)
    {
      ApplyChange(new PokemonCreated(payload));
    }
    private Pokemon()
    {
    }

    public Guid SpeciesId { get; private set; }
    public Guid AbilityId { get; private set; }

    public byte Level { get; private set; }
    public int Experience { get; private set; }

    public PokemonGender Gender { get; private set; }
    public Nature Nature { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    // TODO(fpion): Statistics
    // TODO(fpion): Moves
    public Guid? HeldItemId { get; private set; }

    public History? History { get; private set; }
    // TODO(fpion): Original Trainer
    // TODO(fpion): Current Trainer

    // TODO(fpion): Position
    // TODO(fpion): Box

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public void Delete() => ApplyChange(new PokemonDeleted());

    protected virtual void Apply(PokemonCreated @event)
    {
      SpeciesId = @event.Payload.SpeciesId;
      AbilityId = @event.Payload.AbilityId;

      Level = @event.Payload.Level;
      Experience = @event.Payload.Experience ?? 0; // TODO(fpion): calculate from Level

      Gender = @event.Payload.Gender;
      Nature = Nature.GetNature(@event.Payload.Nature);
      Name = @event.Payload.Name.Trim();
      Description = @event.Payload.Description?.CleanTrim();

      HeldItemId = @event.Payload.HeldItemId;

      History = @event.Payload.History == null ? null : new(@event.Payload.History);

      Notes = @event.Payload.Notes?.CleanTrim();
      Reference = @event.Payload.Reference;
    }
    protected virtual void Apply(PokemonDeleted @event)
    {
      Delete(@event);
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
