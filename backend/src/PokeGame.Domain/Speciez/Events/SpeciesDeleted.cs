using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Speciez.Events;

public record SpeciesDeleted : DomainEvent, IDeleteEvent, INotification;
