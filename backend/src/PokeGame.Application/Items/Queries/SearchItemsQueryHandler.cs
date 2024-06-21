using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Contracts.Items;

namespace PokeGame.Application.Items.Queries;

internal class SearchItemsQueryHandler : IRequestHandler<SearchItemsQuery, SearchResults<Item>>
{
  private readonly IItemQuerier _itemQuerier;

  public SearchItemsQueryHandler(IItemQuerier itemQuerier)
  {
    _itemQuerier = itemQuerier;
  }

  public async Task<SearchResults<Item>> Handle(SearchItemsQuery query, CancellationToken cancellationToken)
  {
    return await _itemQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
