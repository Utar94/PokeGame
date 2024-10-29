using MediatR;
using PokeGame.Contracts.Regions;

namespace PokeGame.Application.Regions.Queries;

public record ReadRegionQuery(Guid Id) : Activity, IRequest<RegionModel?>;

internal class ReadRegionQueryHandler : IRequestHandler<ReadRegionQuery, RegionModel?>
{
  private readonly IRegionQuerier _regionQuerier;

  public ReadRegionQueryHandler(IRegionQuerier regionQuerier)
  {
    _regionQuerier = regionQuerier;
  }

  public async Task<RegionModel?> Handle(ReadRegionQuery query, CancellationToken cancellationToken)
  {
    return await _regionQuerier.ReadAsync(query.Id, cancellationToken);
  }
}
