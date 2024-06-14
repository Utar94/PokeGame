using PokeGame.Contracts.Moves;

namespace PokeGame.EntityFrameworkCore.Entities;

internal class MoveCategoryEntity : EnumEntity
{
  public MoveCategoryEntity(MoveCategory moveCategory) : base(moveCategory)
  {
  }

  private MoveCategoryEntity() : base()
  {
  }
}
