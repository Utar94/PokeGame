using Logitar.Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Configurations;

internal class MoveConfiguration : AggregateConfiguration<MoveEntity>, IEntityTypeConfiguration<MoveEntity>
{
  public override void Configure(EntityTypeBuilder<MoveEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(PokemonContext.Moves));
    builder.HasKey(x => x.MoveId);

    builder.HasIndex(x => x.Type);
    builder.HasIndex(x => x.Category);
    builder.HasIndex(x => x.UniqueName);
    builder.HasIndex(x => x.UniqueNameNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);
    builder.HasIndex(x => x.Accuracy);
    builder.HasIndex(x => x.Power);
    builder.HasIndex(x => x.PowerPoints);

    builder.Ignore(x => x.StatisticChanges);
    builder.Ignore(x => x.StatusConditions);

    builder.Property(x => x.UniqueName).HasMaxLength(UniqueNameUnit.MaximumLength);
    builder.Property(x => x.UniqueNameNormalized).HasMaxLength(UniqueNameUnit.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayNameUnit.MaximumLength);
    builder.Property(x => x.StatisticChangesSerialized).HasColumnName(nameof(MoveEntity.StatisticChanges));
    builder.Property(x => x.StatusConditionsSerialized).HasColumnName(nameof(MoveEntity.StatusConditions));
    builder.Property(x => x.Reference).HasMaxLength(UrlUnit.MaximumLength);
  }
}
