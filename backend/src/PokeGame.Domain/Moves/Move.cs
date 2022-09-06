using PokeGame.Domain.Moves.Events;
using PokeGame.Domain.Moves.Payloads;

namespace PokeGame.Domain.Moves
{
  public class Move : Aggregate
  {
    public Move(CreateMovePayload payload)
    {
      ApplyChange(new MoveCreated(payload));
    }
    private Move()
    {
    }

    public PokemonType Type { get; private set; }
    public MoveCategory Category { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public byte? Accuracy { get; private set; }
    public byte? Power { get; private set; }
    public byte PowerPoints { get; private set; }

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public void Delete() => ApplyChange(new MoveDeleted());
    public void Update(UpdateMovePayload payload) => ApplyChange(new MoveUpdated(payload));

    protected virtual void Apply(MoveCreated @event)
    {
      Type = @event.Payload.Type;
      Category = @event.Payload.Category;

      Apply(@event.Payload);
    }
    protected virtual void Apply(MoveDeleted @event)
    {
      Delete(@event);
    }
    protected virtual void Apply(MoveUpdated @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SaveMovePayload payload)
    {
      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      Accuracy = payload.Accuracy;
      Power = payload.Power;
      PowerPoints = payload.PowerPoints;

      Notes = payload.Notes?.CleanTrim();
      Reference = payload.Reference;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
