using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Configurations;

internal class MoveConfiguration : AggregateConfiguration<MoveEntity>, IEntityTypeConfiguration<MoveEntity>
{
  public override void Configure(EntityTypeBuilder<MoveEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(PokeGameDb.Moves.Table.Table ?? string.Empty, PokeGameDb.Moves.Table.Schema);
    builder.HasKey(x => x.MoveId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.Type);
    builder.HasIndex(x => x.Category);
    builder.HasIndex(x => x.Kind);
    builder.HasIndex(x => x.Name);
    builder.HasIndex(x => x.Accuracy);
    builder.HasIndex(x => x.Power);
    builder.HasIndex(x => x.PowerPoints);

    builder.Property(x => x.Type).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<PokemonType>());
    builder.Property(x => x.Category).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<MoveCategory>());
    builder.Property(x => x.Kind).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<MoveKind>());
    builder.Property(x => x.Name).HasMaxLength(Name.MaximumLength);
    builder.Property(x => x.InflictedStatusCondition).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<StatusCondition>());
    builder.Property(x => x.Link).HasMaxLength(Url.MaximumLength);
  }
}
