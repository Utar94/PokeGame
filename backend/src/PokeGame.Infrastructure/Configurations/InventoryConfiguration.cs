using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Core.Inventories;

namespace PokeGame.Infrastructure.Configurations
{
  internal class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
  {
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
      builder.HasKey(x => new { x.TrainerId, x.ItemId });
    }
  }
}
