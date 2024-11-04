using Logitar.Portal.Contracts.Search;

namespace PokeGame.Contracts.Moves;

public record MoveSortOption : SortOption
{
  public new MoveSort Field
  {
    get => Enum.Parse<MoveSort>(base.Field);
    set => base.Field = value.ToString();
  }

  public MoveSortOption() : this(MoveSort.CreatedOn)
  {
  }

  public MoveSortOption(MoveSort field, bool isDescending = false) : base(field.ToString(), isDescending)
  {
  }
}
