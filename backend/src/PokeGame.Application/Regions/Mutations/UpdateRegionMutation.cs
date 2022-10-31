using MediatR;
using PokeGame.Application.Regions.Models;
using PokeGame.Domain.Regions.Payloads;

namespace PokeGame.Application.Regions.Mutations
{
  public class UpdateRegionMutation : IRequest<RegionModel>
  {
    public UpdateRegionMutation(Guid id, UpdateRegionPayload payload)
    {
      Id = id;
      Payload = payload;
    }

    public Guid Id { get; }
    public UpdateRegionPayload Payload { get; }
  }
}
