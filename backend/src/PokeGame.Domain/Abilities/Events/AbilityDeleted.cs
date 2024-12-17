using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Abilities.Events;

public record AbilityDeleted : DomainEvent, IDeleteEvent, INotification;
