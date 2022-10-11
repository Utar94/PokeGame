using PokeGame.Domain;

namespace PokeGame.ReadModel.Entities
{
  internal abstract class Entity
  {
    public Guid Id { get; set; }
    public int Sid { get; set; }

    public DateTime CreatedOn { get; set; }
    public Guid CreatedById { get; set; }

    public DateTime? UpdatedOn { get; set; }
    public Guid? UpdatedById { get; set; }

    public int Version { get; set; }

    protected void Synchronize(Aggregate aggregate)
    {
      CreatedOn = aggregate.CreatedOn;
      CreatedById = aggregate.CreatedById;

      UpdatedOn = aggregate.UpdatedOn;
      UpdatedById = aggregate.UpdatedById;

      Version = aggregate.Version;
    }
  }
}
