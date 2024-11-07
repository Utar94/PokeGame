﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Configurations;

internal class AbilityConfiguration : AggregateConfiguration<AbilityEntity>, IEntityTypeConfiguration<AbilityEntity>
{
  public override void Configure(EntityTypeBuilder<AbilityEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(PokeGameDb.Abilities.Table.Table ?? string.Empty, PokeGameDb.Abilities.Table.Schema);
    builder.HasKey(x => x.AbilityId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.UniqueName);
    builder.HasIndex(x => x.UniqueNameNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);

    builder.Property(x => x.UniqueName).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.UniqueNameNormalized).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayName.MaximumLength);
    builder.Property(x => x.Link).HasMaxLength(Url.MaximumLength);
  }
}
