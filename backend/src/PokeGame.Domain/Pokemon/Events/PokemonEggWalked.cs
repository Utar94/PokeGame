using MediatR;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonEggWalked : DomainEvent, INotification
  {
    public PokemonEggWalked(ushort steps)
    {
      Steps = steps;
    }

    public ushort Steps { get; private set; }
  }
}
