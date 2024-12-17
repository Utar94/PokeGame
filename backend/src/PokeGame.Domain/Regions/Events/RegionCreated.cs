using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Regions.Events;

public record RegionCreated(UniqueName UniqueName) : DomainEvent, INotification;
