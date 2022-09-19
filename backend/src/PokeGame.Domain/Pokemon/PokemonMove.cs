using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon
{
  public class PokemonMove
  {
    public PokemonMove(PokemonMovePayload payload)
    {
      ArgumentNullException.ThrowIfNull(payload);

      MoveId = payload.MoveId;
      Position = payload.Position;
      RemainingPowerPoints = payload.RemainingPowerPoints;
    }

    public Guid MoveId { get; private set; }
    public byte Position { get; private set; }
    public byte RemainingPowerPoints { get; private set; }

    public void Use()
    {
      if (RemainingPowerPoints == 0)
      {
        throw new InvalidOperationException("The move has no remaining power points.");
      }

      RemainingPowerPoints--;
    }
  }
}
