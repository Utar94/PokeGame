using Logitar.Portal.Contracts.Search;

namespace PokeGame.Contracts.Abilities;

public record AbilitySortOption : SortOption
{
  public new AbilitySort Field
  {
    get => Enum.Parse<AbilitySort>(base.Field);
    set => base.Field = value.ToString();
  }

  public AbilitySortOption() : this(AbilitySort.CreatedOn)
  {
  }

  public AbilitySortOption(AbilitySort field, bool isDescending = false) : base(field.ToString(), isDescending)
  {
  }
}
