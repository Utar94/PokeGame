using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Pokemon;
using PokeGame.ReadModel.Configurations;

namespace PokeGame.ReadModel.Entities.Enums
{
  internal class CharacteristicEntity : EnumEntity
  {
    public CharacteristicEntity(Characteristic characteristic) : base(characteristic)
    {
    }
    private CharacteristicEntity() : base()
    {
    }
  }

  internal class CharacteristicEntityConfiguration : EnumEntityConfiguration<Characteristic, CharacteristicEntity>
  {
    public override void Configure(EntityTypeBuilder<CharacteristicEntity> builder)
    {
      base.Configure(builder);

      builder.ToTable("Characteristics");
    }

    protected override IEnumerable<CharacteristicEntity> GetData()
      => Enum.GetValues<Characteristic>().Select(value => new CharacteristicEntity(value));
  }
}
