using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Speciez.Events;

public record SpeciesCreated(int Number, SpeciesCategory Category, UniqueName UniqueName, GrowthRate GrowthRate, Friendship BaseFriendship, CatchRate CatchRate)
  : DomainEvent, INotification;
