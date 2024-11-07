using Moq;
using PokeGame.Contracts.Regions;

namespace PokeGame.Application.Regions.Queries;

[Trait(Traits.Category, Categories.Unit)]
public class ReadRegionQueryHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IRegionQuerier> _regionQuerier = new();

  private readonly ReadRegionQueryHandler _handler;

  private readonly RegionModel _kanto = new("Kanto")
  {
    Id = Guid.NewGuid()
  };
  private readonly RegionModel _johto = new("Johto")
  {
    Id = Guid.NewGuid()
  };

  public ReadRegionQueryHandlerTests()
  {
    _handler = new(_regionQuerier.Object);

    _regionQuerier.Setup(x => x.ReadAsync(_kanto.Id, _cancellationToken)).ReturnsAsync(_kanto);
    _regionQuerier.Setup(x => x.ReadAsync(_kanto.UniqueName, _cancellationToken)).ReturnsAsync(_kanto);
    _regionQuerier.Setup(x => x.ReadAsync(_johto.Id, _cancellationToken)).ReturnsAsync(_johto);
    _regionQuerier.Setup(x => x.ReadAsync(_johto.UniqueName, _cancellationToken)).ReturnsAsync(_johto);
  }

  [Fact(DisplayName = "It should return null when no region was found.")]
  public async Task It_should_return_null_when_no_region_was_found()
  {
    ReadRegionQuery query = new(Guid.NewGuid(), "Hoenn");
    query.Contextualize();

    Assert.Null(await _handler.Handle(query, _cancellationToken));
  }

  [Fact(DisplayName = "It should return the region found by ID.")]
  public async Task It_should_return_the_region_found_by_Id()
  {
    ReadRegionQuery query = new(_kanto.Id, "Hoenn");
    query.Contextualize();

    RegionModel? region = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(region);
    Assert.Same(_kanto, region);
  }

  [Fact(DisplayName = "It should return the region found by unique name.")]
  public async Task It_should_return_the_region_found_by_unique_name()
  {
    ReadRegionQuery query = new(Guid.NewGuid(), _johto.UniqueName);
    query.Contextualize();

    RegionModel? region = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(region);
    Assert.Same(_johto, region);
  }

  [Fact(DisplayName = "It should throw TooManyResultsException when more than one regions were found.")]
  public async Task It_should_throw_TooManyResultsException_when_more_than_one_regions_were_found()
  {
    ReadRegionQuery query = new(_kanto.Id, _johto.UniqueName);
    query.Contextualize();

    var exception = await Assert.ThrowsAsync<TooManyResultsException<RegionModel>>(async () => await _handler.Handle(query, _cancellationToken));
    Assert.Equal(1, exception.ExpectedCount);
    Assert.Equal(2, exception.ActualCount);
  }
}
