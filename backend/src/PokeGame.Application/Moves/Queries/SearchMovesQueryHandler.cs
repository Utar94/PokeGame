using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Contracts.Moves;

namespace PokeGame.Application.Moves.Queries;

internal class SearchMovesQueryHandler : IRequestHandler<SearchMovesQuery, SearchResults<Move>>
{
  private readonly IMoveQuerier _moveQuerier;

  public SearchMovesQueryHandler(IMoveQuerier moveQuerier)
  {
    _moveQuerier = moveQuerier;
  }

  public async Task<SearchResults<Move>> Handle(SearchMovesQuery query, CancellationToken cancellationToken)
  {
    return await _moveQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
