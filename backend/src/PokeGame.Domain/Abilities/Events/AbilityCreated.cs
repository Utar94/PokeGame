using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Abilities.Events;

public record AbilityCreated(UniqueName UniqueName) : DomainEvent, INotification;
