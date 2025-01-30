using MediatR;
using PokeGame.Application.Moves.Models;

namespace PokeGame.Application.Moves.Queries;

public record ReadMoveQuery(Guid? Id, string? UniqueName) : IRequest<MoveModel?>;

internal class ReadMoveQueryHandler : IRequestHandler<ReadMoveQuery, MoveModel?>
{
  private readonly IMoveQuerier _moveQuerier;

  public ReadMoveQueryHandler(IMoveQuerier moveQuerier)
  {
    _moveQuerier = moveQuerier;
  }

  public async Task<MoveModel?> Handle(ReadMoveQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, MoveModel> moves = new(capacity: 2);

    if (query.Id.HasValue)
    {
      MoveModel? move = await _moveQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (move != null)
      {
        moves[move.Id] = move;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.UniqueName))
    {
      MoveModel? move = await _moveQuerier.ReadAsync(query.UniqueName, cancellationToken);
      if (move != null)
      {
        moves[move.Id] = move;
      }
    }

    if (moves.Count > 1)
    {
      throw TooManyResultsException<MoveModel>.ExpectedSingle(moves.Count);
    }

    return moves.SingleOrDefault().Value;
  }
}
