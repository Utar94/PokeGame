using Moq;
using PokeGame.Application.Regions.Models;

namespace PokeGame.Application.Regions.Queries;

[Trait(Traits.Category, Categories.Unit)]
public class ReadRegionQueryHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IRegionQuerier> _regionQuerier = new();

  private readonly ReadRegionQueryHandler _handler;

  private readonly RegionModel _kanto = new()
  {
    Id = Guid.NewGuid(),
    UniqueName = "Kanto"
  };
  private readonly RegionModel _johto = new()
  {
    Id = Guid.NewGuid(),
    UniqueName = "Johto"
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
  public async Task Given_NoneFound_When_Handle_Then_NullReturned()
  {
    ReadRegionQuery query = new(Id: null, UniqueName: null);
    RegionModel? region = await _handler.Handle(query, _cancellationToken);
    Assert.Null(region);
  }

  [Fact(DisplayName = "It should return the region found by ID.")]
  public async Task Given_FoundById_When_Handle_Then_Returned()
  {
    ReadRegionQuery query = new(_kanto.Id, _kanto.UniqueName);
    RegionModel? region = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(region);
    Assert.Same(_kanto, region);
  }

  [Fact(DisplayName = "It should return the region found by unique name.")]
  public async Task Given_FoundByUniqueName_When_Handle_Then_Returned()
  {
    ReadRegionQuery query = new(Guid.Empty, _kanto.UniqueName);
    RegionModel? region = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(region);
    Assert.Same(_kanto, region);
  }

  [Fact(DisplayName = "It should throw TooManyResultsException when more than one region were found.")]
  public async Task Given_ManyFound_When_Handle_Then_TooManyResultsException()
  {
    ReadRegionQuery query = new(_kanto.Id, _johto.UniqueName);
    var exception = await Assert.ThrowsAsync<TooManyResultsException<RegionModel>>(async () => await _handler.Handle(query, _cancellationToken));
    Assert.Equal(1, exception.ExpectedCount);
    Assert.Equal(2, exception.ActualCount);
  }
}
