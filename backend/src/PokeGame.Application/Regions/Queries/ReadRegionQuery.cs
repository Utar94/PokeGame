using MediatR;
using PokeGame.Application.Regions.Models;

namespace PokeGame.Application.Regions.Queries;

public record ReadRegionQuery(Guid? Id, string? UniqueName) : IRequest<RegionModel?>;

internal class ReadRegionQueryHandler : IRequestHandler<ReadRegionQuery, RegionModel?>
{
  private readonly IRegionQuerier _regionQuerier;

  public ReadRegionQueryHandler(IRegionQuerier regionQuerier)
  {
    _regionQuerier = regionQuerier;
  }

  public async Task<RegionModel?> Handle(ReadRegionQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, RegionModel> regions = new(capacity: 2);

    if (query.Id.HasValue)
    {
      RegionModel? region = await _regionQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (region != null)
      {
        regions[region.Id] = region;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.UniqueName))
    {
      RegionModel? region = await _regionQuerier.ReadAsync(query.UniqueName, cancellationToken);
      if (region != null)
      {
        regions[region.Id] = region;
      }
    }

    if (regions.Count > 1)
    {
      throw TooManyResultsException<RegionModel>.ExpectedSingle(regions.Count);
    }

    return regions.SingleOrDefault().Value;
  }
}
