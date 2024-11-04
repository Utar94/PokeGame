using Logitar.Portal.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Models.Search;

namespace PokeGame.Models.Moves;

public record SearchMovesParameters : SearchParameters
{
  [FromQuery(Name = "type")]
  public PokemonType? Type { get; set; }

  [FromQuery(Name = "category")]
  public MoveCategory? Category { get; set; }

  [FromQuery(Name = "kind")]
  public MoveKind? Kind { get; set; }

  public SearchMovesPayload ToPayload()
  {
    SearchMovesPayload payload = new()
    {
      Type = Type,
      Category = Category,
      Kind = Kind
    };
    Fill(payload);

    foreach (SortOption sort in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(sort.Field, out MoveSort field))
      {
        payload.Sort.Add(new MoveSortOption(field, sort.IsDescending));
      }
    }

    return payload;
  }
}
