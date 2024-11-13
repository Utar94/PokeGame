namespace PokeGame.Contracts.Species;

public record UpdatePokedexNumberPayload
{
  public Guid RegionId { get; set; }
  public int? Number { get; set; }

  public UpdatePokedexNumberPayload() : this(regionId: Guid.Empty, number: null)
  {
  }

  public UpdatePokedexNumberPayload(Guid regionId, int? number)
  {
    RegionId = regionId;
    Number = number;
  }
}
