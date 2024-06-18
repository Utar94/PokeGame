using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;
using PokeGame.Contracts.Items;

namespace PokeGame.Domain.Items.Events;

public class ItemCreatedEvent : DomainEvent, INotification
{
  public ItemCategory Category { get; }

  public UniqueNameUnit UniqueName { get; }

  public ItemCreatedEvent(ItemCategory category, UniqueNameUnit uniqueName)
  {
    Category = category;

    UniqueName = uniqueName;
  }
}
