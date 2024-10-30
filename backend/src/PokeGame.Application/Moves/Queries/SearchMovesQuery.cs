using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Contracts.Moves;

namespace PokeGame.Application.Moves.Queries;

public record SearchMovesQuery(SearchMovesPayload Payload) : Activity, IRequest<SearchResults<MoveModel>>;

internal class SearchMovesQueryHandler : IRequestHandler<SearchMovesQuery, SearchResults<MoveModel>>
{
  private readonly IMoveQuerier _moveQuerier;

  public SearchMovesQueryHandler(IMoveQuerier moveQuerier)
  {
    _moveQuerier = moveQuerier;
  }

  public async Task<SearchResults<MoveModel>> Handle(SearchMovesQuery query, CancellationToken cancellationToken)
  {
    return await _moveQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
