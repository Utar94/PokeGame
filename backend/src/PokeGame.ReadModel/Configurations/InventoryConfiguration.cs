using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Configurations
{
  internal class InventoryConfiguration : IEntityTypeConfiguration<InventoryEntity>
  {
    public void Configure(EntityTypeBuilder<InventoryEntity> builder)
    {
      builder.HasKey(x => new { x.TrainerId, x.ItemId });
    }
  }
}
