using MediatR;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonWounded : DomainEvent, INotification
  {
    public PokemonWounded(ushort damage, StatusCondition? statusCondition, byte? attackerLevel = null)
    {
      AttackerLevel = attackerLevel;
      Damage = damage;
      StatusCondition = statusCondition;
    }

    public byte? AttackerLevel { get; private set; }
    public ushort Damage { get; private set; }
    public StatusCondition? StatusCondition { get; private set; }
  }
}
