using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Moves.Events;

public record MoveDeleted : DomainEvent, IDeleteEvent, INotification;
