using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon.Mutations
{
  public class EvolvePokemonMutation : IRequest<PokemonModel>
  {
    public EvolvePokemonMutation(Guid id, EvolvePokemonPayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public EvolvePokemonPayload Payload { get; }
  }
}
