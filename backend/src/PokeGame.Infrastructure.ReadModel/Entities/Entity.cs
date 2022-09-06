using PokeGame.Domain;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal abstract class Entity
  {
    public Guid Id { get; set; }
    public int Sid { get; set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedById { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedById { get; set; }

    public int Version { get; set; }

    protected void Synchronize(Aggregate aggregate)
    {
      CreatedAt = aggregate.CreatedAt;
      CreatedById = aggregate.CreatedById;

      UpdatedAt = aggregate.UpdatedAt;
      UpdatedById = aggregate.UpdatedById;

      Version = aggregate.Version;
    }
  }
}
