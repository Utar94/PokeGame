using Logitar.Portal.Contracts.Search;
using Moq;
using PokeGame.Contracts.Moves;

namespace PokeGame.Application.Moves.Queries;

[Trait(Traits.Category, Categories.Unit)]
public class SearchMovesQueryHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IMoveQuerier> _moveQuerier = new();

  private readonly SearchMovesQueryHandler _handler;

  public SearchMovesQueryHandlerTests()
  {
    _handler = new(_moveQuerier.Object);
  }

  [Fact(DisplayName = "It should return the search results.")]
  public async Task It_should_return_the_search_results()
  {
    SearchMovesPayload payload = new();
    SearchMovesQuery query = new(payload);
    query.Contextualize();

    SearchResults<MoveModel> results = new();
    _moveQuerier.Setup(x => x.SearchAsync(payload, _cancellationToken)).ReturnsAsync(results);

    SearchResults<MoveModel> moves = await _handler.Handle(query, _cancellationToken);
    Assert.Same(results, moves);
  }
}
