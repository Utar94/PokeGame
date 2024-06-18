using MediatR;
using PokeGame.Contracts.Regions;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

internal class DeleteRegionCommandHandler : IRequestHandler<DeleteRegionCommand, Region?>
{
  private readonly IRegionQuerier _regionQuerier;
  private readonly IRegionRepository _regionRepository;

  public DeleteRegionCommandHandler(IRegionQuerier regionQuerier, IRegionRepository regionRepository)
  {
    _regionQuerier = regionQuerier;
    _regionRepository = regionRepository;
  }

  public async Task<Region?> Handle(DeleteRegionCommand command, CancellationToken cancellationToken)
  {
    RegionId id = new(command.Id);
    RegionAggregate? region = await _regionRepository.LoadAsync(id, cancellationToken);
    if (region == null)
    {
      return null;
    }
    Region result = await _regionQuerier.ReadAsync(region, cancellationToken);

    region.Delete(command.ActorId);

    await _regionRepository.SaveAsync(region, cancellationToken);

    return result;
  }
}
