using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain;
using PokeGame.Domain.Moves;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Configurations
{
  internal class MoveConfiguration : EntityConfiguration<MoveEntity>, IEntityTypeConfiguration<MoveEntity>
  {
    public override void Configure(EntityTypeBuilder<MoveEntity> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.AccuracyStage).HasDefaultValue(0);
      builder.Property(x => x.Category).HasDefaultValue(default(MoveCategory));
      builder.Property(x => x.EvasionStage).HasDefaultValue(0);
      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.PowerPoints).HasDefaultValue(0);
      builder.Property(x => x.Reference).HasMaxLength(2048);
      builder.Property(x => x.Sid).HasColumnName("MoveId");
      builder.Property(x => x.StatisticStages).HasMaxLength(100);
      builder.Property(x => x.Type).HasDefaultValue(default(PokemonType));
    }
  }
}
