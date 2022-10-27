using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Pokemon;
using PokeGame.ReadModel.Configurations;

namespace PokeGame.ReadModel.Entities.Enums
{
  internal class LevelingRateEntity : EnumEntity
  {
    public LevelingRateEntity(LevelingRate levelingRate) : base(levelingRate)
    {
    }
    private LevelingRateEntity() : base()
    {
    }
  }

  internal class LevelingRateEntityConfiguration : EnumEntityConfiguration<LevelingRate, LevelingRateEntity>
  {
    public override void Configure(EntityTypeBuilder<LevelingRateEntity> builder)
    {
      base.Configure(builder);

      builder.ToTable("LevelingRates");
    }

    protected override IEnumerable<LevelingRateEntity> GetData()
      => Enum.GetValues<LevelingRate>().Select(value => new LevelingRateEntity(value));
  }
}
