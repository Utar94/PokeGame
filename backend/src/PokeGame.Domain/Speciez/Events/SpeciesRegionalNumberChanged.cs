using Logitar.EventSourcing;
using MediatR;
using PokeGame.Domain.Regions;

namespace PokeGame.Domain.Speciez.Events;

public record SpeciesRegionalNumberChanged(RegionId RegionId, int? Number) : DomainEvent, INotification;
