using Logitar.Portal.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Speciez.Models;
using PokeGame.Domain;
using PokeGame.Domain.Speciez;
using PokeGame.Models.Search;

namespace PokeGame.Models.Species;

public record SearchSpeciesParameters : SearchParameters
{
  [FromQuery(Name = "category")]
  public SpeciesCategory? Category { get; set; }

  [FromQuery(Name = "growth")]
  public GrowthRate? GrowthRate { get; set; }

  [FromQuery(Name = "region")]
  public Guid? RegionId { get; set; }

  public SearchSpeciesPayload ToPayload()
  {
    SearchSpeciesPayload payload = new()
    {
      Category = Category,
      GrowthRate = GrowthRate,
      RegionId = RegionId
    };
    Fill(payload);

    foreach (SortOption sort in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(sort.Field, out SpeciesSort field))
      {
        payload.Sort.Add(new SpeciesSortOption(field, sort.IsDescending));
      }
    }

    return payload;
  }
}
