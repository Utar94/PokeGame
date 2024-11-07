using Logitar.EventSourcing;
using MediatR;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

internal record SaveMoveCommand(Move Move) : IRequest;

internal class SaveMoveCommandHandler : IRequestHandler<SaveMoveCommand>
{
  private readonly IMoveQuerier _moveQuerier;
  private readonly IMoveRepository _moveRepository;

  public SaveMoveCommandHandler(IMoveQuerier moveQuerier, IMoveRepository moveRepository)
  {
    _moveQuerier = moveQuerier;
    _moveRepository = moveRepository;
  }

  public async Task Handle(SaveMoveCommand command, CancellationToken cancellationToken)
  {
    Move move = command.Move;

    bool hasUniqueNameChanged = false;
    foreach (DomainEvent change in move.Changes)
    {
      if (change is Move.CreatedEvent || change is Move.UpdatedEvent updatedEvent && updatedEvent.UniqueName != null)
      {
        hasUniqueNameChanged = true;
        break;
      }
    }

    if (hasUniqueNameChanged)
    {
      MoveId? moveId = await _moveQuerier.FindIdAsync(move.UniqueName, cancellationToken);
      if (moveId != null && moveId != move.Id)
      {
        throw new UniqueNameAlreadyUsedException(move, moveId.Value);
      }
    }

    await _moveRepository.SaveAsync(move, cancellationToken);
  }
}
