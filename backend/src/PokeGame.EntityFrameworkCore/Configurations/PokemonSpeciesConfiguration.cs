using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokeGame.Contracts.Species;
using PokeGame.Domain;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Configurations;

internal class PokemonSpeciesConfiguration : AggregateConfiguration<PokemonSpeciesEntity>, IEntityTypeConfiguration<PokemonSpeciesEntity>
{
  public override void Configure(EntityTypeBuilder<PokemonSpeciesEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(PokeGameDb.PokemonSpecies.Table.Table ?? string.Empty, PokeGameDb.PokemonSpecies.Table.Schema);
    builder.HasKey(x => x.PokemonSpeciesId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.Number).IsUnique();
    builder.HasIndex(x => x.UniqueName);
    builder.HasIndex(x => x.UniqueNameNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);
    builder.HasIndex(x => x.Category);
    builder.HasIndex(x => x.BaseHappiness);
    builder.HasIndex(x => x.CaptureRate);
    builder.HasIndex(x => x.LevelingRate);

    builder.Property(x => x.UniqueName).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.UniqueNameNormalized).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayName.MaximumLength);
    builder.Property(x => x.Category).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<PokemonCategory>());
    builder.Property(x => x.LevelingRate).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<LevelingRate>());
    builder.Property(x => x.Link).HasMaxLength(Url.MaximumLength);
  }
}
