using Logitar.EventSourcing;
using MediatR;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

internal record SaveRegionCommand(Region Region) : IRequest;

internal class SaveRegionCommandHandler : IRequestHandler<SaveRegionCommand>
{
  private readonly IRegionQuerier _regionQuerier;
  private readonly IRegionRepository _regionRepository;

  public SaveRegionCommandHandler(IRegionQuerier regionQuerier, IRegionRepository regionRepository)
  {
    _regionQuerier = regionQuerier;
    _regionRepository = regionRepository;
  }

  public async Task Handle(SaveRegionCommand command, CancellationToken cancellationToken)
  {
    Region region = command.Region;

    bool hasUniqueNameChanged = false;
    foreach (DomainEvent change in region.Changes)
    {
      if (change is Region.CreatedEvent || change is Region.UpdatedEvent updatedEvent && updatedEvent.UniqueName != null)
      {
        hasUniqueNameChanged = true;
        break;
      }
    }

    if (hasUniqueNameChanged)
    {
      RegionId? regionId = await _regionQuerier.FindIdAsync(region.UniqueName, cancellationToken);
      if (regionId != null && regionId != region.Id)
      {
        throw new UniqueNameAlreadyUsedException(region, regionId.Value);
      }
    }

    await _regionRepository.SaveAsync(region, cancellationToken);
  }
}
