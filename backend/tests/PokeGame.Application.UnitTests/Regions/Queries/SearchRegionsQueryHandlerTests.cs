using Logitar.Portal.Contracts.Search;
using Moq;
using PokeGame.Contracts.Regions;

namespace PokeGame.Application.Regions.Queries;

[Trait(Traits.Category, Categories.Unit)]
public class SearchRegionsQueryHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IRegionQuerier> _regionQuerier = new();

  private readonly SearchRegionsQueryHandler _handler;

  public SearchRegionsQueryHandlerTests()
  {
    _handler = new(_regionQuerier.Object);
  }

  [Fact(DisplayName = "It should return the search results.")]
  public async Task It_should_return_the_search_results()
  {
    SearchRegionsPayload payload = new();
    SearchRegionsQuery query = new(payload);
    query.Contextualize();

    SearchResults<RegionModel> results = new();
    _regionQuerier.Setup(x => x.SearchAsync(payload, _cancellationToken)).ReturnsAsync(results);

    SearchResults<RegionModel> regions = await _handler.Handle(query, _cancellationToken);
    Assert.Same(results, regions);
  }
}
