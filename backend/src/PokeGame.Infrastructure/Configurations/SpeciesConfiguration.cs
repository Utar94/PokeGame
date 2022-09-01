using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Core;
using PokeGame.Core.Pokemon;
using PokeGame.Core.Species;
using PokeGame.Infrastructure.Entities;

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

      builder.HasMany(x => x.Abilities).WithMany(x => x.Species)
        .UsingEntity<SpeciesAbility>(builder => builder.HasKey(x => new { x.SpeciesId, x.AbilityId }));

      builder.Ignore(x => x.BaseStatistics);
      builder.Ignore(x => x.EvYield);

      builder.Property(x => x.BaseFriendship).HasDefaultValue(0);
      builder.Property(x => x.BaseStatisticsSerialized).HasColumnName(nameof(Species.BaseStatistics)).HasMaxLength(100);
      builder.Property(x => x.Category).HasMaxLength(100);
      builder.Property(x => x.EvYieldSerialized).HasColumnName(nameof(Species.EvYield)).HasMaxLength(50);
      builder.Property(x => x.LevelingRate).HasDefaultValue(default(LevelingRate));
      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.PrimaryType).HasDefaultValue(default(PokemonType));
      builder.Property(x => x.Reference).HasMaxLength(2048);
      builder.Property(x => x.Sid).HasColumnName($"{nameof(Species)}Id");
    }
  }
}
