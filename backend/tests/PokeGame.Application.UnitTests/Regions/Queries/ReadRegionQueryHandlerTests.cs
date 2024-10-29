using Moq;
using PokeGame.Contracts.Regions;

namespace PokeGame.Application.Regions.Queries;

[Trait(Traits.Category, Categories.Unit)]
public class ReadRegionQueryHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IRegionQuerier> _regionQuerier = new();

  private readonly ReadRegionQueryHandler _handler;

  private readonly RegionModel _region = new("Kanto")
  {
    Id = Guid.NewGuid()
  };

  public ReadRegionQueryHandlerTests()
  {
    _handler = new(_regionQuerier.Object);

    _regionQuerier.Setup(x => x.ReadAsync(_region.Id, _cancellationToken)).ReturnsAsync(_region);
  }

  [Fact(DisplayName = "It should return null when no region was found.")]
  public async Task It_should_return_null_when_no_region_was_found()
  {
    ReadRegionQuery query = new(Guid.NewGuid());
    query.Contextualize();

    Assert.Null(await _handler.Handle(query, _cancellationToken));
  }

  [Fact(DisplayName = "It should return the region found by ID.")]
  public async Task It_should_return_the_region_found_by_Id()
  {
    ReadRegionQuery query = new(_region.Id);
    query.Contextualize();

    RegionModel? region = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(region);
    Assert.Same(_region, region);
  }
}
