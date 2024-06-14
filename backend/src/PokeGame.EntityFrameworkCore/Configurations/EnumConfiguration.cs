using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Configurations;

internal abstract class EnumConfiguration<T> where T : EnumEntity
{
  public virtual void Configure(EntityTypeBuilder<T> builder)
  {
    builder.HasKey(x => x.Id);

    builder.HasIndex(x => x.Name).IsUnique();

    builder.Property(x => x.Id).ValueGeneratedNever();
    builder.Property(x => x.Name).HasMaxLength(byte.MaxValue);

    builder.HasData(GetData());
  }

  protected abstract IEnumerable<T> GetData();
}
