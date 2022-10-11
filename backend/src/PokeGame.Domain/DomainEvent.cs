namespace PokeGame.Domain
{
  public abstract class DomainEvent
  {
    public Guid AggregateId { get; set; }
    public DateTime OccurredOn { get; set; }
    public Guid UserId { get; set; }
    public int Version { get; set; }
  }
}
