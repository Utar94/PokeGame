using Logitar.Portal.Contracts.Search;

namespace PokeGame.Contracts.Species;

public record SpeciesSortOption : SortOption
{
  public new SpeciesSort Field
  {
    get => Enum.Parse<SpeciesSort>(base.Field);
    set => base.Field = value.ToString();
  }

  public SpeciesSortOption() : this(SpeciesSort.CreatedOn)
  {
  }

  public SpeciesSortOption(SpeciesSort field, bool isDescending = false) : base(field.ToString(), isDescending)
  {
  }
}
