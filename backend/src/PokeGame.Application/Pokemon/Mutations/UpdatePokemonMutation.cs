using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon.Mutations
{
  public class UpdatePokemonMutation : IRequest<PokemonModel>
  {
    public UpdatePokemonMutation(Guid id, UpdatePokemonPayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public UpdatePokemonPayload Payload { get; }
  }
}
