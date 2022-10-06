using MediatR;
using PokeGame.Application.Species.Models;
using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Application.Species.Mutations
{
  public class UpdateSpeciesMutation : IRequest<SpeciesModel>
  {
    public UpdateSpeciesMutation(Guid id, UpdateSpeciesPayload payload)
    {
      Id = id;
      Payload = payload;
    }

    public Guid Id { get; }
    public UpdateSpeciesPayload Payload { get; }
  }
}
