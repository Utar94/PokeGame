using Logitar.Portal.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Contracts.Items;
using PokeGame.Models.Search;

namespace PokeGame.Models.Items;

public record SearchItemsParameters : SearchParameters
{
  [FromQuery(Name = "category")]
  public ItemCategory? Category { get; set; }

  public SearchItemsPayload ToPayload()
  {
    SearchItemsPayload payload = new()
    {
      Category = Category
    };

    FillPayload(payload);

    List<SortOption>? sortOptions = ((SearchPayload)payload).Sort;
    if (sortOptions != null)
    {
      payload.Sort = new List<ItemSortOption>(capacity: sortOptions.Count);
      foreach (SortOption sort in sortOptions)
      {
        if (Enum.TryParse(sort.Field, out ItemSort field))
        {
          payload.Sort.Add(new ItemSortOption(field, sort.IsDescending));
        }
      }
    }

    return payload;
  }
}
