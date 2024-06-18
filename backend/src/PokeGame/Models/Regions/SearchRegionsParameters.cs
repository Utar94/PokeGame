using Logitar.Portal.Contracts.Search;
using PokeGame.Contracts.Regions;
using PokeGame.Models.Search;

namespace PokeGame.Models.Regions;

public record SearchRegionsParameters : SearchParameters
{
  public SearchRegionsPayload ToPayload()
  {
    SearchRegionsPayload payload = new();

    FillPayload(payload);

    List<SortOption>? sortOptions = ((SearchPayload)payload).Sort;
    if (sortOptions != null)
    {
      payload.Sort = new List<RegionSortOption>(capacity: sortOptions.Count);
      foreach (SortOption sort in sortOptions)
      {
        if (Enum.TryParse(sort.Field, out RegionSort field))
        {
          payload.Sort.Add(new RegionSortOption(field, sort.IsDescending));
        }
      }
    }

    return payload;
  }
}
