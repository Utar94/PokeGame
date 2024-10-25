using Logitar.Portal.Contracts.Search;
using Moq;
using PokeGame.Contracts.Abilities;

namespace PokeGame.Application.Abilities.Queries;

[Trait(Traits.Category, Categories.Unit)]
public class SearchAbilitiesQueryHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IAbilityQuerier> _abilityQuerier = new();

  private readonly SearchAbilitiesQueryHandler _handler;

  public SearchAbilitiesQueryHandlerTests()
  {
    _handler = new(_abilityQuerier.Object);
  }

  [Fact(DisplayName = "It should return the search results.")]
  public async Task It_should_return_the_search_results()
  {
    SearchAbilitiesPayload payload = new();
    SearchAbilitiesQuery query = new(payload);
    query.Contextualize();

    SearchResults<AbilityModel> results = new();
    _abilityQuerier.Setup(x => x.SearchAsync(payload, _cancellationToken)).ReturnsAsync(results);

    SearchResults<AbilityModel> abilities = await _handler.Handle(query, _cancellationToken);
    Assert.Same(results, abilities);
  }
}
