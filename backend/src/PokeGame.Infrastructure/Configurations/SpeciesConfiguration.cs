using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Core;
using PokeGame.Core.Species;

namespace PokeGame.Infrastructure.Configurations
{
  internal class SpeciesConfiguration : AggregateConfiguration<Species>, IEntityTypeConfiguration<Species>
  {
    public override void Configure(EntityTypeBuilder<Species> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Category);
      builder.HasIndex(x => x.Name);
      builder.HasIndex(x => x.Number).IsUnique();
      builder.HasIndex(x => x.PrimaryType);
      builder.HasIndex(x => x.SecondaryType);

      builder.Property(x => x.Category).HasMaxLength(100);
      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.PrimaryType).HasDefaultValue(default(PokemonType));
      builder.Property(x => x.Reference).HasMaxLength(2048);
    }
  }
}
