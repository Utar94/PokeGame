using Logitar.EventSourcing;
using MediatR;
using PokeGame.Domain.Regions;
using PokeGame.Domain.Regions.Events;

namespace PokeGame.Application.Regions.Commands;

internal class SaveRegionCommandHandler : IRequestHandler<SaveRegionCommand>
{
  private readonly IRegionRepository _regionRepository;

  public SaveRegionCommandHandler(IRegionRepository regionRepository)
  {
    _regionRepository = regionRepository;
  }

  public async Task Handle(SaveRegionCommand command, CancellationToken cancellationToken)
  {
    RegionAggregate region = command.Region;

    bool hasUniqueNameChanged = false;
    foreach (DomainEvent change in region.Changes)
    {
      if (change is RegionCreatedEvent || (change is RegionUpdatedEvent updated && updated.UniqueName != null))
      {
        hasUniqueNameChanged = true;
      }
    }

    if (hasUniqueNameChanged)
    {
      RegionAggregate? other = await _regionRepository.LoadAsync(region.UniqueName, cancellationToken);
      if (other != null && !other.Equals(region))
      {
        throw new UniqueNameAlreadyUsedException<RegionAggregate>(region.UniqueName, nameof(region.UniqueName));
      }
    }

    await _regionRepository.SaveAsync(region, cancellationToken);
  }
}
