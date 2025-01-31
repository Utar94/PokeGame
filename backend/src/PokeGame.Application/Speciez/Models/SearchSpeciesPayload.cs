using Logitar.Portal.Contracts.Search;
using PokeGame.Domain;
using PokeGame.Domain.Speciez;

namespace PokeGame.Application.Speciez.Models;

public record SearchSpeciesPayload : SearchPayload
{
  public SpeciesCategory? Category { get; set; }
  public GrowthRate? GrowthRate { get; set; }
  public Guid? RegionId { get; set; }

  public new List<SpeciesSortOption> Sort { get; set; } = [];
}
