using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon.Mutations
{
  public class WalkPokemonEggMutation : IRequest<PokemonModel>
  {
    public WalkPokemonEggMutation(Guid id, WalkPokemonEggPayload payload)
    {
      Id = id;
      Payload = payload;
    }

    public Guid Id { get; private set; }
    public WalkPokemonEggPayload Payload { get; private set; }
  }
}
