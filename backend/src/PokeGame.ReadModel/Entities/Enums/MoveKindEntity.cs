using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Moves;
using PokeGame.ReadModel.Configurations;

namespace PokeGame.ReadModel.Entities.Enums
{
  internal class MoveKindEntity : EnumEntity
  {
    public MoveKindEntity(MoveKind moveType) : base(moveType)
    {
    }
    private MoveKindEntity() : base()
    {
    }
  }

  internal class MoveKindEntityConfiguration : EnumEntityConfiguration<MoveKind, MoveKindEntity>
  {
    public override void Configure(EntityTypeBuilder<MoveKindEntity> builder)
    {
      base.Configure(builder);

      builder.ToTable("MoveKinds");
    }

    protected override IEnumerable<MoveKindEntity> GetData()
      => Enum.GetValues<MoveKind>().Select(value => new MoveKindEntity(value));
  }
}
