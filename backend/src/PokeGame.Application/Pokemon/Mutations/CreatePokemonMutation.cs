using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon.Mutations
{
  public class CreatePokemonMutation : IRequest<PokemonModel>
  {
    public CreatePokemonMutation(CreatePokemonPayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreatePokemonPayload Payload { get; }
  }
}
