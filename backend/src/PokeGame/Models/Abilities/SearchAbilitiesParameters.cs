using Logitar.Portal.Contracts.Search;
using PokeGame.Contracts.Abilities;
using PokeGame.Models.Search;

namespace PokeGame.Models.Abilities;

public record SearchAbilitiesParameters : SearchParameters
{
  public SearchAbilitiesPayload ToPayload()
  {
    SearchAbilitiesPayload payload = new();

    FillPayload(payload);

    List<SortOption>? sortOptions = ((SearchPayload)payload).Sort;
    if (sortOptions != null)
    {
      payload.Sort = new List<AbilitySortOption>(capacity: sortOptions.Count);
      foreach (SortOption sort in sortOptions)
      {
        if (Enum.TryParse(sort.Field, out AbilitySort field))
        {
          payload.Sort.Add(new AbilitySortOption(field, sort.IsDescending));
        }
      }
    }

    return payload;
  }
}
