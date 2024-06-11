using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Abilities.Events;

public class AbilityDeletedEvent : DomainEvent, INotification;
