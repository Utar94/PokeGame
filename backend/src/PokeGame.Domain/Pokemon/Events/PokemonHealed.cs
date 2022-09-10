using MediatR;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonHealed : DomainEvent, INotification
  {
    public PokemonHealed(short restoreHitPoints, bool removeCondition)
    {
      RestoreHitPoints = restoreHitPoints;
      RemoveCondition = removeCondition;
    }

    public short RestoreHitPoints { get; private set; }
    public bool RemoveCondition { get; private set; }
  }
}
