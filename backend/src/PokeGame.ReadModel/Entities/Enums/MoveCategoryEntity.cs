using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Moves;
using PokeGame.ReadModel.Configurations;

namespace PokeGame.ReadModel.Entities.Enums
{
  internal class MoveCategoryEntity : EnumEntity
  {
    public MoveCategoryEntity(MoveCategory moveCategory) : base(moveCategory)
    {
    }
    private MoveCategoryEntity() : base()
    {
    }
  }

  internal class MoveCategoryEntityConfiguration : EnumEntityConfiguration<MoveCategory, MoveCategoryEntity>
  {
    public override void Configure(EntityTypeBuilder<MoveCategoryEntity> builder)
    {
      base.Configure(builder);

      builder.ToTable("MoveCategories");
    }

    protected override IEnumerable<MoveCategoryEntity> GetData()
      => Enum.GetValues<MoveCategory>().Select(value => new MoveCategoryEntity(value));
  }
}
