using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon.Mutations
{
  public class CatchPokemonMutation : IRequest<PokemonModel>
  {
    public CatchPokemonMutation(Guid id, CatchPokemonPayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public CatchPokemonPayload Payload { get; }
  }
}
