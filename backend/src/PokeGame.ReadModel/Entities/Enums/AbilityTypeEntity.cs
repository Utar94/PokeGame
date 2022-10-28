using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Abilities;
using PokeGame.ReadModel.Configurations;

namespace PokeGame.ReadModel.Entities.Enums
{
  internal class AbilityTypeEntity : EnumEntity
  {
    public AbilityTypeEntity(AbilityType abilityType) : base(abilityType)
    {
    }
    private AbilityTypeEntity() : base()
    {
    }
  }

  internal class AbilityTypeEntityConfiguration : EnumEntityConfiguration<AbilityType, AbilityTypeEntity>
  {
    public override void Configure(EntityTypeBuilder<AbilityTypeEntity> builder)
    {
      base.Configure(builder);

      builder.ToTable("AbilityTypes");
    }

    protected override IEnumerable<AbilityTypeEntity> GetData()
      => Enum.GetValues<AbilityType>().Select(value => new AbilityTypeEntity(value));
  }
}
