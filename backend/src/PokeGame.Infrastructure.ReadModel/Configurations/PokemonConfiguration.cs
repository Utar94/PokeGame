using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Pokemon;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Configurations
{
  internal class PokemonConfiguration : EntityConfiguration<PokemonEntity>, IEntityTypeConfiguration<PokemonEntity>
  {
    public override void Configure(EntityTypeBuilder<PokemonEntity> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Gender);
      builder.HasIndex(x => x.Surname);

      builder.HasOne(x => x.CurrentTrainer).WithMany(x => x.Pokemon)
        .HasForeignKey(x => x.CurrentTrainerId);
      builder.HasOne(x => x.OriginalTrainer).WithMany(x => x.OriginalPokemon)
        .HasForeignKey(x => x.OriginalTrainerId);

      builder.Property(x => x.EffortValues).HasMaxLength(100);
      builder.Property(x => x.Experience).HasDefaultValue(0);
      builder.Property(x => x.Gender).HasDefaultValue(default(PokemonGender));
      builder.Property(x => x.IndividualValues).HasMaxLength(100);
      builder.Property(x => x.MetLocation).HasMaxLength(100);
      builder.Property(x => x.Nature).HasMaxLength(10);
      builder.Property(x => x.Surname).HasMaxLength(100);
      builder.Property(x => x.Reference).HasMaxLength(2048);
      builder.Property(x => x.Sid).HasColumnName("PokemonId");
      builder.Property(x => x.Statistics).HasMaxLength(100);
    }
  }
}
