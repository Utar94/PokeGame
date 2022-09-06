using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Configurations
{
  internal class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
  {
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
      builder.HasKey(x => new { x.TrainerId, x.ItemId });
    }
  }
}
