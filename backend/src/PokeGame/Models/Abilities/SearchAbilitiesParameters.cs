﻿using Logitar.Portal.Contracts.Search;
using PokeGame.Contracts.Abilities;
using PokeGame.Models.Search;

namespace PokeGame.Models.Abilities;

public record SearchAbilitiesParameters : SearchParameters
{
  public SearchAbilitiesPayload ToPayload()
  {
    SearchAbilitiesPayload payload = new();
    Fill(payload);

    foreach (SortOption sort in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(sort.Field, out AbilitySort field))
      {
        payload.Sort.Add(new AbilitySortOption(field, sort.IsDescending));
      }
    }

    return payload;
  }
}
