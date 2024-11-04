using MediatR;
using PokeGame.Contracts.Moves;

namespace PokeGame.Application.Moves.Queries;

public record ReadMoveQuery(Guid Id) : Activity, IRequest<MoveModel?>;

internal class ReadMoveQueryHandler : IRequestHandler<ReadMoveQuery, MoveModel?>
{
  private readonly IMoveQuerier _moveQuerier;

  public ReadMoveQueryHandler(IMoveQuerier moveQuerier)
  {
    _moveQuerier = moveQuerier;
  }

  public async Task<MoveModel?> Handle(ReadMoveQuery query, CancellationToken cancellationToken)
  {
    return await _moveQuerier.ReadAsync(query.Id, cancellationToken);
  }
}
