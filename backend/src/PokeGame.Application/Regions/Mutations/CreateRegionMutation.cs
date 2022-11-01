using MediatR;
using PokeGame.Application.Regions.Models;
using PokeGame.Domain.Regions.Payloads;

namespace PokeGame.Application.Regions.Mutations
{
  public class CreateRegionMutation : IRequest<RegionModel>
  {
    public CreateRegionMutation(CreateRegionPayload payload)
    {
      Payload = payload;
    }

    public CreateRegionPayload Payload { get; }
  }
}
