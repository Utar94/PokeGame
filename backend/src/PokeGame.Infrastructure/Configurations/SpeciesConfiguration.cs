using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokeGame.Domain;
using PokeGame.Domain.Speciez;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure.Configurations;

internal class SpeciesConfiguration : AggregateConfiguration<SpeciesEntity>, IEntityTypeConfiguration<SpeciesEntity>
{
  public override void Configure(EntityTypeBuilder<SpeciesEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(PokeGameDb.Species.Table.Table ?? string.Empty, PokeGameDb.Species.Table.Schema);
    builder.HasKey(x => x.SpeciesId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.Number).IsUnique();
    builder.HasIndex(x => x.Category);
    builder.HasIndex(x => x.UniqueName);
    builder.HasIndex(x => x.UniqueNameNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);
    builder.HasIndex(x => x.GrowthRate);

    builder.Property(x => x.Category).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<SpeciesCategory>());
    builder.Property(x => x.UniqueName).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.UniqueNameNormalized).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayName.MaximumLength);
    builder.Property(x => x.GrowthRate).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<GrowthRate>());
    builder.Property(x => x.Link).HasMaxLength(Url.MaximumLength);
  }
}
