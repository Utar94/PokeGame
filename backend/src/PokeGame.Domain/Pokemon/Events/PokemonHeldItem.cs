using MediatR;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonHeldItem : DomainEvent, INotification
  {
    public PokemonHeldItem(Guid? itemId)
    {
      ItemId = itemId;
    }

    public Guid? ItemId { get; }
  }
}
