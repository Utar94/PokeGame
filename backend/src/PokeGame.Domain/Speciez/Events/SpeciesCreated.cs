using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Speciez.Events;

public record SpeciesCreated(int Number, SpeciesCategory Category, UniqueName UniqueName) : DomainEvent, INotification;
