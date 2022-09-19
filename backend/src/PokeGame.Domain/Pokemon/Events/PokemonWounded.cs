using MediatR;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonWounded : DomainEvent, INotification
  {
    public PokemonWounded(short damage, StatusCondition? statusCondition)
    {
      Damage = damage;
      StatusCondition = statusCondition;
    }

    public short Damage { get; private set; }
    public StatusCondition? StatusCondition { get; private set; }
  }
}
