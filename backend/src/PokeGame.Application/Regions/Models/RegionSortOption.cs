﻿using Logitar.Portal.Contracts.Search;

namespace PokeGame.Application.Regions.Models;

public record RegionSortOption : SortOption
{
  public new RegionSort Field
  {
    get => Enum.Parse<RegionSort>(base.Field);
    set => base.Field = value.ToString();
  }

  public RegionSortOption() : this(RegionSort.DisplayName)
  {
  }

  public RegionSortOption(RegionSort field, bool isDescending = false) : base(field.ToString(), isDescending)
  {
  }
}
