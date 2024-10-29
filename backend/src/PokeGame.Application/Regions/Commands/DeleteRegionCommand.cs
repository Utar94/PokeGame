using MediatR;
using PokeGame.Contracts.Regions;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

public record DeleteRegionCommand(Guid Id) : Activity, IRequest<RegionModel?>;

internal class DeleteRegionCommandHandler : IRequestHandler<DeleteRegionCommand, RegionModel?>
{
  private readonly IRegionQuerier _regionQuerier;
  private readonly IRegionRepository _regionRepository;

  public DeleteRegionCommandHandler(IRegionQuerier regionQuerier, IRegionRepository regionRepository)
  {
    _regionQuerier = regionQuerier;
    _regionRepository = regionRepository;
  }

  public async Task<RegionModel?> Handle(DeleteRegionCommand command, CancellationToken cancellationToken)
  {
    RegionId id = new(command.Id);
    Region? region = await _regionRepository.LoadAsync(id, cancellationToken);
    if (region == null)
    {
      return null;
    }
    RegionModel model = await _regionQuerier.ReadAsync(region, cancellationToken);

    region.Delete(command.GetUserId());

    await _regionRepository.SaveAsync(region, cancellationToken);

    return model;
  }
}
