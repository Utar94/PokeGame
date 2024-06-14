using Logitar.EventSourcing;
using MediatR;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Moves.Events;

namespace PokeGame.Application.Moves.Commands;

internal class SaveMoveCommandHandler : IRequestHandler<SaveMoveCommand>
{
  private readonly IMoveRepository _moveRepository;

  public SaveMoveCommandHandler(IMoveRepository moveRepository)
  {
    _moveRepository = moveRepository;
  }

  public async Task Handle(SaveMoveCommand command, CancellationToken cancellationToken)
  {
    MoveAggregate move = command.Move;

    bool hasUniqueNameChanged = false;
    foreach (DomainEvent change in move.Changes)
    {
      if (change is MoveCreatedEvent || (change is MoveUpdatedEvent updated && updated.UniqueName != null))
      {
        hasUniqueNameChanged = true;
      }
    }

    if (hasUniqueNameChanged)
    {
      MoveAggregate? other = await _moveRepository.LoadAsync(move.UniqueName, cancellationToken);
      if (other != null && !other.Equals(move))
      {
        throw new UniqueNameAlreadyUsedException<MoveAggregate>(move.UniqueName, nameof(move.UniqueName));
      }
    }

    await _moveRepository.SaveAsync(move, cancellationToken);
  }
}
