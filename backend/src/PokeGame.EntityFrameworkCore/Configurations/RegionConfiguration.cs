using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Configurations;

internal class RegionConfiguration : AggregateConfiguration<RegionEntity>, IEntityTypeConfiguration<RegionEntity>
{
  public override void Configure(EntityTypeBuilder<RegionEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(PokeGameDb.Regions.Table.Table ?? string.Empty, PokeGameDb.Regions.Table.Schema);
    builder.HasKey(x => x.RegionId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.Name);

    builder.Property(x => x.Name).HasMaxLength(Name.MaximumLength);
    builder.Property(x => x.Link).HasMaxLength(Url.MaximumLength);
  }
}
