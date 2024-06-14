using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Moves.Events;

public class MoveDeletedEvent : DomainEvent, INotification;
