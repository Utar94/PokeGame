using Logitar.Portal.Contracts.Search;

namespace PokeGame.Application.Abilities.Models;

public record AbilitySortOption : SortOption
{
  public new AbilitySort Field
  {
    get => Enum.Parse<AbilitySort>(base.Field);
    set => base.Field = value.ToString();
  }

  public AbilitySortOption() : this(AbilitySort.DisplayName)
  {
  }

  public AbilitySortOption(AbilitySort field, bool isDescending = false) : base(field.ToString(), isDescending)
  {
  }
}
