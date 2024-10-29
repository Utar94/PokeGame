using Logitar.Portal.Contracts.Search;
using PokeGame.Contracts.Regions;
using PokeGame.Models.Search;

namespace PokeGame.Models.Regions;

public record SearchRegionsParameters : SearchParameters
{
  public SearchRegionsPayload ToPayload()
  {
    SearchRegionsPayload payload = new();
    Fill(payload);

    foreach (SortOption sort in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(sort.Field, out RegionSort field))
      {
        payload.Sort.Add(new RegionSortOption(field, sort.IsDescending));
      }
    }

    return payload;
  }
}
