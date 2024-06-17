using MediatR;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

internal class DeleteMoveCommandHandler : IRequestHandler<DeleteMoveCommand, Move?>
{
  private readonly IMoveQuerier _moveQuerier;
  private readonly IMoveRepository _moveRepository;

  public DeleteMoveCommandHandler(IMoveQuerier moveQuerier, IMoveRepository moveRepository)
  {
    _moveQuerier = moveQuerier;
    _moveRepository = moveRepository;
  }

  public async Task<Move?> Handle(DeleteMoveCommand command, CancellationToken cancellationToken)
  {
    MoveId id = new(command.Id);
    MoveAggregate? move = await _moveRepository.LoadAsync(id, cancellationToken);
    if (move == null)
    {
      return null;
    }
    Move result = await _moveQuerier.ReadAsync(move, cancellationToken);

    move.Delete(command.ActorId);

    await _moveRepository.SaveAsync(move, cancellationToken);

    return result;
  }
}
