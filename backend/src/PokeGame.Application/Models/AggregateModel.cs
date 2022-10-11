namespace PokeGame.Application.Models
{
  public abstract class AggregateModel
  {
    public Guid Id { get; set; }
    public ActorModel? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public ActorModel? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public int Version { get; set; }
  }
}
