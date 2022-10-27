using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain;
using PokeGame.ReadModel.Configurations;

namespace PokeGame.ReadModel.Entities.Enums
{
  internal class TimeOfDayEntity : EnumEntity
  {
    public TimeOfDayEntity(TimeOfDay timeOfDay) : base(timeOfDay)
    {
    }
    private TimeOfDayEntity() : base()
    {
    }
  }

  internal class TimeOfDayEntityConfiguration : EnumEntityConfiguration<TimeOfDay, TimeOfDayEntity>
  {
    public override void Configure(EntityTypeBuilder<TimeOfDayEntity> builder)
    {
      base.Configure(builder);

      builder.ToTable("TimesOfDay");
    }

    protected override IEnumerable<TimeOfDayEntity> GetData()
      => Enum.GetValues<TimeOfDay>().Select(value => new TimeOfDayEntity(value));
  }
}
