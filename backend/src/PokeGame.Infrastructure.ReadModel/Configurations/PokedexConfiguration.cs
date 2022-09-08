using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Configurations
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
