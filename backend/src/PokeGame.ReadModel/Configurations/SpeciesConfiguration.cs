using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain;
using PokeGame.Domain.Pokemon;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Configurations
{
  internal class SpeciesConfiguration : EntityConfiguration<SpeciesEntity>, IEntityTypeConfiguration<SpeciesEntity>
  {
    public override void Configure(EntityTypeBuilder<SpeciesEntity> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Category);
      builder.HasIndex(x => x.Name);
      builder.HasIndex(x => x.Number).IsUnique();
      builder.HasIndex(x => x.PrimaryType);
      builder.HasIndex(x => x.SecondaryType);

      builder.Property(x => x.BaseFriendship).HasDefaultValue(0);
      builder.Property(x => x.BaseStatistics).HasMaxLength(100);
      builder.Property(x => x.Category).HasMaxLength(100);
      builder.Property(x => x.EvYield).HasMaxLength(50);
      builder.Property(x => x.LevelingRate).HasDefaultValue(default(LevelingRate));
      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.Picture).HasMaxLength(2048);
      builder.Property(x => x.PictureFemale).HasMaxLength(2048);
      builder.Property(x => x.PictureShiny).HasMaxLength(2048);
      builder.Property(x => x.PictureShinyFemale).HasMaxLength(2048);
      builder.Property(x => x.PrimaryType).HasDefaultValue(default(PokemonType));
      builder.Property(x => x.Reference).HasMaxLength(2048);
      builder.Property(x => x.Sid).HasColumnName("SpeciesId");
    }
  }
}
