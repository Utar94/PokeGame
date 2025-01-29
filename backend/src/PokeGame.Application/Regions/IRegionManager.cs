using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions;

public interface IRegionManager
{
  Task SaveAsync(Region region, CancellationToken cancellationToken = default);
}
