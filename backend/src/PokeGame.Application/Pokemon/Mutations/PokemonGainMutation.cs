using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon.Mutations
{
  public class PokemonGainMutation : IRequest<PokemonModel>
  {
    public PokemonGainMutation(Guid id, ExperienceGainPayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public ExperienceGainPayload Payload { get; }
  }
}
