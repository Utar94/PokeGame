using Logitar.Portal.Contracts.Search;
using Moq;
using PokeGame.Application.Abilities.Models;

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

  [Fact(DisplayName = "It should return the correct search results.")]
  public async Task Given_Payload_When_Handle_Then_ResultsReturned()
  {
    SearchAbilitiesPayload payload = new();
    SearchResults<AbilityModel> results = new();
    _abilityQuerier.Setup(x => x.SearchAsync(payload, _cancellationToken)).ReturnsAsync(results);

    SearchAbilitiesQuery query = new(payload);
    SearchResults<AbilityModel> abilities = await _handler.Handle(query, _cancellationToken);
    Assert.Same(results, abilities);
  }
}
