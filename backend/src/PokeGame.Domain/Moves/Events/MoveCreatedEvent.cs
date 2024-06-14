using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;

namespace PokeGame.Domain.Moves.Events;

public class MoveCreatedEvent : DomainEvent, INotification
{
  public PokemonType Type { get; }
  public MoveCategory Category { get; }

  public UniqueNameUnit UniqueName { get; }

  public int PowerPoints { get; }

  public MoveCreatedEvent(PokemonType type, MoveCategory category, UniqueNameUnit uniqueName, int powerPoints)
  {
    Type = type;
    Category = category;

    UniqueName = uniqueName;

    PowerPoints = powerPoints;
  }
}
