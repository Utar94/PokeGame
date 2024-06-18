using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Items.Events;

public class ItemDeletedEvent : DomainEvent, INotification;
