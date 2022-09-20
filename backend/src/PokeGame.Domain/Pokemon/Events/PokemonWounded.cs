using MediatR;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonWounded : DomainEvent, INotification
  {
    public PokemonWounded(ushort damage, StatusCondition? statusCondition)
    {
      Damage = damage;
      StatusCondition = statusCondition;
    }

    public ushort Damage { get; private set; }
    public StatusCondition? StatusCondition { get; private set; }
  }
}
