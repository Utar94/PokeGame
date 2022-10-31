using MediatR;
using PokeGame.Application.Regions.Models;

namespace PokeGame.Application.Regions.Queries
{
  public class GetRegionQuery : IRequest<RegionModel?>
  {
    public GetRegionQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
