using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain;
using PokeGame.Domain.Trainers;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Configurations
{
  internal class TrainerConfiguration : EntityConfiguration<TrainerEntity>, IEntityTypeConfiguration<TrainerEntity>
  {
    public override void Configure(EntityTypeBuilder<TrainerEntity> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Gender);
      builder.HasIndex(x => x.Name);
      builder.HasIndex(x => x.LegacyRegion);
      builder.HasIndex(x => new { x.LegacyRegion, x.Number, x.Name }).IsUnique();

      builder.Property(x => x.Checksum).HasDefaultValue(0);
      builder.Property(x => x.Gender).HasDefaultValue(default(TrainerGender));
      builder.Property(x => x.Money).HasDefaultValue(0);
      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.NationalPokedex).HasDefaultValue(false);
      builder.Property(x => x.Picture).HasMaxLength(2048);
      builder.Property(x => x.PlayTime).HasDefaultValue(0);
      builder.Property(x => x.PokedexCount).HasDefaultValue(0);
      builder.Property(x => x.Reference).HasMaxLength(2048);
      builder.Property(x => x.LegacyRegion).HasDefaultValue(default(Region));
      builder.Property(x => x.Sid).HasColumnName("TrainerId");
    }
  }
}
