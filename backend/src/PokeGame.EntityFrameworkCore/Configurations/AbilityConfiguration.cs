using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Configurations;

internal class AbilityConfiguration : AggregateConfiguration<AbilityEntity>, IEntityTypeConfiguration<AbilityEntity>
{
  public override void Configure(EntityTypeBuilder<AbilityEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(PokemonContext.Abilities));
    builder.HasKey(x => x.AbilityId);

    builder.HasIndex(x => x.UniqueName);
    builder.HasIndex(x => x.UniqueNameNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);

    builder.Property(x => x.UniqueName).HasMaxLength(UniqueNameUnit.MaximumLength);
    builder.Property(x => x.UniqueNameNormalized).HasMaxLength(UniqueNameUnit.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayNameUnit.MaximumLength);
  }
}
