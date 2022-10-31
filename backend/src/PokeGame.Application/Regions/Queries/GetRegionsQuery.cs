using MediatR;
using PokeGame.Application.Models;
using PokeGame.Application.Regions.Models;

namespace PokeGame.Application.Regions.Queries
{
  public class GetRegionsQuery : IRequest<ListModel<RegionModel>>
  {
    public string? Search { get; set; }

    public RegionSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
