using MediatR;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

public record DeleteMoveCommand(Guid Id) : Activity, IRequest<MoveModel?>;

internal class DeleteMoveCommandHandler : IRequestHandler<DeleteMoveCommand, MoveModel?>
{
  private readonly IMoveQuerier _moveQuerier;
  private readonly IMoveRepository _moveRepository;

  public DeleteMoveCommandHandler(IMoveQuerier moveQuerier, IMoveRepository moveRepository)
  {
    _moveQuerier = moveQuerier;
    _moveRepository = moveRepository;
  }

  public async Task<MoveModel?> Handle(DeleteMoveCommand command, CancellationToken cancellationToken)
  {
    MoveId id = new(command.Id);
    Move? move = await _moveRepository.LoadAsync(id, cancellationToken);
    if (move == null)
    {
      return null;
    }
    MoveModel model = await _moveQuerier.ReadAsync(move, cancellationToken);

    move.Delete(command.GetUserId());

    await _moveRepository.SaveAsync(move, cancellationToken);

    return model;
  }
}
