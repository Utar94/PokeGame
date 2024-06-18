using MediatR;
using PokeGame.Contracts.Regions;

namespace PokeGame.Application.Regions.Queries;

internal class ReadRegionQueryHandler : IRequestHandler<ReadRegionQuery, Region?>
{
  private readonly IRegionQuerier _regionQuerier;

  public ReadRegionQueryHandler(IRegionQuerier regionQuerier)
  {
    _regionQuerier = regionQuerier;
  }

  public async Task<Region?> Handle(ReadRegionQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, Region> regions = new(capacity: 2);

    if (query.Id.HasValue)
    {
      Region? region = await _regionQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (region != null)
      {
        regions[region.Id] = region;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.UniqueName))
    {
      Region? region = await _regionQuerier.ReadAsync(query.UniqueName, cancellationToken);
      if (region != null)
      {
        regions[region.Id] = region;
      }
    }

    if (regions.Count > 1)
    {
      throw TooManyResultsException<Region>.ExpectedSingle(regions.Count);
    }

    return regions.Values.SingleOrDefault();
  }
}
