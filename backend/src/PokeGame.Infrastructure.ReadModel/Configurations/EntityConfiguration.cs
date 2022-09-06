using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Configurations
{
  internal abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
  {
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
      builder.HasKey(x => x.Sid);
      builder.HasIndex(x => x.Id).IsUnique();

      builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
      builder.Property(x => x.CreatedById).HasDefaultValue(Guid.Empty);
      builder.Property(x => x.Id).HasDefaultValueSql("uuid_generate_v4()");
      builder.Property(x => x.Version).HasDefaultValue(0);
    }
  }
}
