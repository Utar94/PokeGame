using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Core.Abilities;

namespace PokeGame.Infrastructure.Configurations
{
  internal class AbilityConfiguration : AggregateConfiguration<Ability>, IEntityTypeConfiguration<Ability>
  {
    public override void Configure(EntityTypeBuilder<Ability> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.Reference).HasMaxLength(2048);
      builder.Property(x => x.Sid).HasColumnName($"{nameof(Ability)}Id");
    }
  }
}
