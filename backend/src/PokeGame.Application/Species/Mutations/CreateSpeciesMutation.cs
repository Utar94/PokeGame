using MediatR;
using PokeGame.Application.Species.Models;
using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Application.Species.Mutations
{
  public class CreateSpeciesMutation : IRequest<SpeciesModel>
  {
    public CreateSpeciesMutation(CreateSpeciesPayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateSpeciesPayload Payload { get; }
  }
}
