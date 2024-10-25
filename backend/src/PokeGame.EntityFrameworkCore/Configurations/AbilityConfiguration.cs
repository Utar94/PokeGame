using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokeGame.Contracts.Abilities;
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
    builder.HasIndex(x => x.Kind);
    builder.HasIndex(x => x.Name);

    builder.Property(x => x.Kind).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<AbilityKind>());
    builder.Property(x => x.Name).HasMaxLength(Name.MaximumLength);
    builder.Property(x => x.Link).HasMaxLength(Url.MaximumLength);
  }
}
