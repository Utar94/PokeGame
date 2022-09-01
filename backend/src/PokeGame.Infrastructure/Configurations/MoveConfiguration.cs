using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Core;
using PokeGame.Core.Moves;

namespace PokeGame.Infrastructure.Configurations
{
  internal class MoveConfiguration : AggregateConfiguration<Move>, IEntityTypeConfiguration<Move>
  {
    public override void Configure(EntityTypeBuilder<Move> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Category).HasDefaultValue(default(MoveCategory));
      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.PowerPoints).HasDefaultValue(0);
      builder.Property(x => x.Reference).HasMaxLength(2048);
      builder.Property(x => x.Sid).HasColumnName($"{nameof(Move)}Id");
      builder.Property(x => x.Type).HasDefaultValue(default(PokemonType));
    }
  }
}
