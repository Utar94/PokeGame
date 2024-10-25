using PokeGame.Contracts.Abilities;
using PokeGame.Domain.Abilities;

namespace PokeGame.EntityFrameworkCore.Entities;

internal class AbilityEntity : AggregateEntity
{
  public int AbilityId { get; private set; }
  public Guid Id { get; private set; }

  public AbilityKind? Kind { get; private set; }

  public string Name { get; private set; } = string.Empty;
  public string? Description { get; private set; }

  public string? Link { get; private set; }
  public string? Notes { get; private set; }

  public AbilityEntity(Ability.CreatedEvent @event) : base(@event)
  {
    Id = @event.AggregateId.ToGuid();

    Name = @event.Name.Value;
  }

  private AbilityEntity() : base()
  {
  }

  public void Update(Ability.UpdatedEvent @event)
  {
    if (@event.Kind != null)
    {
      Kind = @event.Kind.Value;
    }

    if (@event.Name != null)
    {
      Name = @event.Name.Value;
    }
    if (@event.Description != null)
    {
      Description = @event.Description.Value?.Value;
    }

    if (@event.Link != null)
    {
      Link = @event.Link.Value?.Value;
    }
    if (@event.Notes != null)
    {
      Notes = @event.Notes.Value?.Value;
    }
  }

  public override string ToString() => $"{Name} | {base.ToString()}";
}
