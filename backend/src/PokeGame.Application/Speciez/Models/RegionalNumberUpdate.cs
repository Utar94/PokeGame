namespace PokeGame.Application.Speciez.Models;

public record RegionalNumberUpdate
{
  public Guid RegionId { get; set; }
  public int? Number { get; set; }
}
