namespace PokeGame.Application.Speciez.Models;

public record RegionalNumberPayload
{
  public Guid RegionId { get; set; }
  public int Number { get; set; }
}
