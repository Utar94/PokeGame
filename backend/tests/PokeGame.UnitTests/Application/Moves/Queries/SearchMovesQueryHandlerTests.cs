using Logitar.Portal.Contracts.Search;
using Moq;
using PokeGame.Application.Moves.Models;

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

  [Fact(DisplayName = "It should return the correct search results.")]
  public async Task Given_Payload_When_Handle_Then_ResultsReturned()
  {
    SearchMovesPayload payload = new();
    SearchResults<MoveModel> results = new();
    _moveQuerier.Setup(x => x.SearchAsync(payload, _cancellationToken)).ReturnsAsync(results);

    SearchMovesQuery query = new(payload);
    SearchResults<MoveModel> moves = await _handler.Handle(query, _cancellationToken);
    Assert.Same(results, moves);
  }
}
