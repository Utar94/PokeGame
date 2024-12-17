using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Moves.Events;

public record MoveCreated(PokemonType Type, MoveCategory Category, UniqueName UniqueName, PowerPoints PowerPoints) : DomainEvent, INotification;
