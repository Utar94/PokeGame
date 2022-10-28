using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Abilities;
using PokeGame.ReadModel.Configurations;

namespace PokeGame.ReadModel.Entities.Enums
{
  internal class AbilityKindEntity : EnumEntity
  {
    public AbilityKindEntity(AbilityKind abilityKind) : base(abilityKind)
    {
    }
    private AbilityKindEntity() : base()
    {
    }
  }

  internal class AbilityKindEntityConfiguration : EnumEntityConfiguration<AbilityKind, AbilityKindEntity>
  {
    public override void Configure(EntityTypeBuilder<AbilityKindEntity> builder)
    {
      base.Configure(builder);

      builder.ToTable("AbilityKinds");
    }

    protected override IEnumerable<AbilityKindEntity> GetData()
      => Enum.GetValues<AbilityKind>().Select(value => new AbilityKindEntity(value));
  }
}
