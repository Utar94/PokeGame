using Logitar.Portal.Contracts.Search;
using Moq;
using PokeGame.Application.Speciez.Models;

namespace PokeGame.Application.Speciez.Queries;

[Trait(Traits.Category, Categories.Unit)]
public class SearchSpeciesQueryHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<ISpeciesQuerier> _speciesQuerier = new();

  private readonly SearchSpeciesQueryHandler _handler;

  public SearchSpeciesQueryHandlerTests()
  {
    _handler = new(_speciesQuerier.Object);
  }

  [Fact(DisplayName = "It should return the correct search results.")]
  public async Task Given_Payload_When_Handle_Then_ResultsReturned()
  {
    SearchSpeciesPayload payload = new();
    SearchResults<SpeciesModel> results = new();
    _speciesQuerier.Setup(x => x.SearchAsync(payload, _cancellationToken)).ReturnsAsync(results);

    SearchSpeciesQuery query = new(payload);
    SearchResults<SpeciesModel> speciess = await _handler.Handle(query, _cancellationToken);
    Assert.Same(results, speciess);
  }
}
