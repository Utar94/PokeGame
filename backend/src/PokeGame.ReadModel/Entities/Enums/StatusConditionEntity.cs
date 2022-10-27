using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Pokemon;
using PokeGame.ReadModel.Configurations;

namespace PokeGame.ReadModel.Entities.Enums
{
  internal class StatusConditionEntity : EnumEntity
  {
    public StatusConditionEntity(StatusCondition statusCondition) : base(statusCondition)
    {
    }
    private StatusConditionEntity() : base()
    {
    }
  }

  internal class StatusConditionEntityConfiguration : EnumEntityConfiguration<StatusCondition, StatusConditionEntity>
  {
    public override void Configure(EntityTypeBuilder<StatusConditionEntity> builder)
    {
      base.Configure(builder);

      builder.ToTable("StatusConditions");
    }

    protected override IEnumerable<StatusConditionEntity> GetData()
      => Enum.GetValues<StatusCondition>().Select(value => new StatusConditionEntity(value));
  }
}
