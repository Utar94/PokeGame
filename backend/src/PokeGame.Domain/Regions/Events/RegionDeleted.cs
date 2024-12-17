using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Regions.Events;

public record RegionDeleted : DomainEvent, IDeleteEvent, INotification;
