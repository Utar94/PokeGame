using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Configurations
{
  internal class PokedexConfiguration : IEntityTypeConfiguration<PokedexEntity>
  {
    public void Configure(EntityTypeBuilder<PokedexEntity> builder)
    {
      builder.HasIndex(x => x.HasCaught);

      builder.HasKey(x => new { x.TrainerId, x.SpeciesId });
    }
  }
}
