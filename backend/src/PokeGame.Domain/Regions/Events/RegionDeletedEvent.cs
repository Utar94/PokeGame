using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Regions.Events;

public class RegionDeletedEvent : DomainEvent, INotification;
