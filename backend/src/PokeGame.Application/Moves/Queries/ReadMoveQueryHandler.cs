using MediatR;
using PokeGame.Contracts.Moves;

namespace PokeGame.Application.Moves.Queries;

internal class ReadMoveQueryHandler : IRequestHandler<ReadMoveQuery, Move?>
{
  private readonly IMoveQuerier _moveQuerier;

  public ReadMoveQueryHandler(IMoveQuerier moveQuerier)
  {
    _moveQuerier = moveQuerier;
  }

  public async Task<Move?> Handle(ReadMoveQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, Move> moves = new(capacity: 2);

    if (query.Id.HasValue)
    {
      Move? move = await _moveQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (move != null)
      {
        moves[move.Id] = move;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.UniqueName))
    {
      Move? move = await _moveQuerier.ReadAsync(query.UniqueName, cancellationToken);
      if (move != null)
      {
        moves[move.Id] = move;
      }
    }

    if (moves.Count > 1)
    {
      throw TooManyResultsException<Move>.ExpectedSingle(moves.Count);
    }

    return moves.Values.SingleOrDefault();
  }
}
