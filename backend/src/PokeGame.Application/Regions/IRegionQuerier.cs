using PokeGame.Application.Models;
using PokeGame.Application.Regions.Models;

namespace PokeGame.Application.Regions
{
  public interface IRegionQuerier
  {
    Task<RegionModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<RegionModel>> GetPagedAsync(string? search = null,
      RegionSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
  }
}
