﻿using Logitar.Portal.Contracts.Search;
using Moq;
using PokeGame.Application.Regions.Models;

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

  [Fact(DisplayName = "It should return the correct search results.")]
  public async Task Given_Payload_When_Handle_Then_ResultsReturned()
  {
    SearchRegionsPayload payload = new();
    SearchResults<RegionModel> results = new();
    _regionQuerier.Setup(x => x.SearchAsync(payload, _cancellationToken)).ReturnsAsync(results);

    SearchRegionsQuery query = new(payload);
    SearchResults<RegionModel> regions = await _handler.Handle(query, _cancellationToken);
    Assert.Same(results, regions);
  }
}
