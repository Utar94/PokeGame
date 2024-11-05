using PokeGame.Contracts.Regions;

namespace PokeGame.Seeding.Worker.Backend;

internal record RegionPayload : CreateOrReplaceRegionPayload
{
  public Guid Id { get; set; }
}
