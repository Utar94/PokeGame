using PokeGame.Core.Moves.Events;
using PokeGame.Core.Moves.Payloads;

namespace PokeGame.Core.Moves
{
  public class Move : Aggregate
  {
    public Move(CreateMovePayload payload, Guid userId)
    {
      ApplyChange(new CreatedEvent(payload, userId));
    }
    private Move()
    {
    }

    public PokemonType Type { get; private set; }
    public MoveCategory Category { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public byte? Accuracy { get; private set; }
    public byte? Power { get; private set; }
    public byte PowerPoints { get; private set; }

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateMovePayload payload, Guid userId) => ApplyChange(new UpdatedEvent(payload, userId));

    protected virtual void Apply(CreatedEvent @event)
    {
      Type = @event.Payload.Type;
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
