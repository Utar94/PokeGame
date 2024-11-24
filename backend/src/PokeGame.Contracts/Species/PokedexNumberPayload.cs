namespace PokeGame.Contracts.Species;

public record PokedexNumberPayload
{
  public Guid RegionId { get; set; }
  public int Number { get; set; }

  public PokedexNumberPayload() : this(regionId: Guid.Empty, number: 0)
  {
  }

  public PokedexNumberPayload(Guid regionId, int number)
  {
    RegionId = regionId;
    Number = number;
  }
}
