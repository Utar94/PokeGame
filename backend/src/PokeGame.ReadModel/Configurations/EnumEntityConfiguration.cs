using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.ReadModel.Entities.Enums;

namespace PokeGame.ReadModel.Configurations
{
  internal abstract class EnumEntityConfiguration<TEnum, TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : EnumEntity
  {
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
      builder.HasIndex(x => x.Name).IsUnique();

      builder.Property(x => x.Id).ValueGeneratedNever();
      builder.Property(x => x.Name).HasMaxLength(256);

      builder.HasData(GetData());
    }

    protected abstract IEnumerable<TEntity> GetData();
  }
}
