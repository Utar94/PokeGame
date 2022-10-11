using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Configurations
{
  internal class PokemonMoveConfiguration : IEntityTypeConfiguration<PokemonMoveEntity>
  {
    public void Configure(EntityTypeBuilder<PokemonMoveEntity> builder)
    {
      builder.HasKey(x => new { x.PokemonId, x.MoveId });
      builder.HasIndex(x => new { x.PokemonId, x.Position }).IsUnique();
    }
  }
}
