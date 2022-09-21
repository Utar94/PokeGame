using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Species;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Configurations
{
  internal class EvolutionConfiguration : IEntityTypeConfiguration<EvolutionEntity>
  {
    public void Configure(EntityTypeBuilder<EvolutionEntity> builder)
    {
      builder.HasKey(x => new { x.EvolvingSpeciesId, x.EvolvedSpeciesId });

      builder.HasOne(x => x.EvolvedSpecies).WithMany(x => x.EvolvedFrom);
      builder.HasOne(x => x.EvolvingSpecies).WithMany(x => x.Evolutions);

      builder.Property(x => x.HighFriendship).HasDefaultValue(false);
      builder.Property(x => x.Level).HasDefaultValue(0);
      builder.Property(x => x.Location).HasMaxLength(100);
      builder.Property(x => x.Method).HasDefaultValue(default(EvolutionMethod));
    }
  }
}
