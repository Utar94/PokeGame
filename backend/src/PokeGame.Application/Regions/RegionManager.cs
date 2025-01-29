using Logitar.EventSourcing;
using PokeGame.Domain;
using PokeGame.Domain.Regions;
using PokeGame.Domain.Regions.Events;

namespace PokeGame.Application.Regions;

internal class RegionManager : IRegionManager
{
  private readonly IRegionQuerier _regionQuerier;
  private readonly IRegionRepository _regionRepository;

  public RegionManager(IRegionQuerier regionQuerier, IRegionRepository regionRepository)
  {
    _regionQuerier = regionQuerier;
    _regionRepository = regionRepository;
  }

  public async Task SaveAsync(Region region, CancellationToken cancellationToken)
  {
    UniqueName? uniqueName = null;
    foreach (IEvent change in region.Changes)
    {
      if (change is RegionCreated created)
      {
        uniqueName = created.UniqueName;
      }
      else if (change is RegionUpdated updated && updated.UniqueName != null)
      {
        uniqueName = updated.UniqueName;
      }
    }

    if (uniqueName != null)
    {
      RegionId? conflictId = await _regionQuerier.FindIdAsync(uniqueName, cancellationToken);
      if (conflictId.HasValue && !conflictId.Value.Equals(region.Id))
      {
        throw new UniqueNameAlreadyUsedException(region, conflictId.Value);
      }
    }

    await _regionRepository.SaveAsync(region, cancellationToken);
  }
}
