using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Application.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon.Mutations
{
  public class BattleGainMutation : IRequest<IEnumerable<PokemonModel>>
  {
    public BattleGainMutation(BattleGainPayload payload)
    {
      Payload = payload;
    }

    public BattleGainPayload Payload { get; }
  }
}
