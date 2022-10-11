using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Configurations
{
  internal class PokemonPositionConfiguration : IEntityTypeConfiguration<PokemonPositionEntity>
  {
    public void Configure(EntityTypeBuilder<PokemonPositionEntity> builder)
    {
      builder.HasKey(x => x.PokemonId);
      builder.HasIndex(x => new { x.TrainerId, x.Position, x.Box }).IsUnique();

      builder.Property(x => x.Box).HasDefaultValue(0);
      builder.Property(x => x.Position).HasDefaultValue(0);
    }
  }
}
