using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Trainers;
using PokeGame.ReadModel.Configurations;

namespace PokeGame.ReadModel.Entities.Enums
{
  internal class TrainerGenderEntity : EnumEntity
  {
    public TrainerGenderEntity(TrainerGender trainerGender) : base(trainerGender)
    {
    }
    private TrainerGenderEntity() : base()
    {
    }
  }

  internal class TrainerGenderEntityConfiguration : EnumEntityConfiguration<TrainerGender, TrainerGenderEntity>
  {
    public override void Configure(EntityTypeBuilder<TrainerGenderEntity> builder)
    {
      base.Configure(builder);

      builder.ToTable("TrainerGenders");
    }

    protected override IEnumerable<TrainerGenderEntity> GetData()
      => Enum.GetValues<TrainerGender>().Select(value => new TrainerGenderEntity(value));
  }
}
