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

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateTrainerPayload payload, Guid userId) => ApplyChange(new UpdatedEvent(payload, userId));

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
