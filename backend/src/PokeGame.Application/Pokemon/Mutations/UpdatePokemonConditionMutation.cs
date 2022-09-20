using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon.Mutations
{
  public class UpdatePokemonConditionMutation : IRequest<PokemonModel>
  {
    public UpdatePokemonConditionMutation(Guid id, UpdatePokemonConditionPayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public UpdatePokemonConditionPayload Payload { get; }
  }
}
