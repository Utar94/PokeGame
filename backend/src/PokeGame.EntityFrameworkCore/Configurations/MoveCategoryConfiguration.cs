using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Contracts.Moves;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Configurations;

internal class MoveCategoryConfiguration : EnumConfiguration<MoveCategoryEntity>, IEntityTypeConfiguration<MoveCategoryEntity>
{
  public override void Configure(EntityTypeBuilder<MoveCategoryEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(PokemonContext.MoveCategories));

    builder.Property(x => x.Id).HasColumnName(PokemonDb.MoveCategories.MoveCategoryId.Name);
  }

  protected override IEnumerable<MoveCategoryEntity> GetData()
  {
    return Enum.GetValues<MoveCategory>().Select(value => new MoveCategoryEntity(value));
  }
}
