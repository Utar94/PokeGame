using MediatR;
using PokeGame.Application.Pokedex.Payloads;

namespace PokeGame.Application.Pokedex.Mutations
{
  public class SeenSpeciesMutation : IRequest
  {
    public SeenSpeciesMutation(SeenSpeciesPayload payload)
    {
      Payload = payload;
    }

    public SeenSpeciesPayload Payload { get; }
  }
}
