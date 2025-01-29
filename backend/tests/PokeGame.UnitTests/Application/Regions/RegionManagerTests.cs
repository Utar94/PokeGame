using Moq;
using PokeGame.Domain;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions;

[Trait(Traits.Category, Categories.Unit)]
public class RegionManagerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IRegionQuerier> _regionQuerier = new();
  private readonly Mock<IRegionRepository> _regionRepository = new();

  private readonly RegionManager _manager;

  private readonly Region _region = new(new UniqueName("Kanto"));

  public RegionManagerTests()
  {
    _manager = new(_regionQuerier.Object, _regionRepository.Object);
  }

  [Fact(DisplayName = "SaveAsync: it should not query regions when the unique did not change.")]
  public async Task Given_NoChange_When_SaveAsync_Then_NotQueried()
  {
    _region.ClearChanges();

    await _manager.SaveAsync(_region, _cancellationToken);

    _regionQuerier.Verify(x => x.FindIdAsync(It.IsAny<UniqueName>(), It.IsAny<CancellationToken>()), Times.Never());
    _regionRepository.Verify(x => x.SaveAsync(_region, _cancellationToken), Times.Once());
  }

  [Fact(DisplayName = "SaveAsync: it should save the region when there is no conflict.")]
  public async Task Given_NoConflict_When_SaveAsync_Then_RegionSaved()
  {
    _regionQuerier.Setup(x => x.FindIdAsync(_region.UniqueName, _cancellationToken)).ReturnsAsync(_region.Id);

    await _manager.SaveAsync(_region, _cancellationToken);

    _regionQuerier.Verify(x => x.FindIdAsync(_region.UniqueName, _cancellationToken), Times.Once());
    _regionRepository.Verify(x => x.SaveAsync(_region, _cancellationToken), Times.Once());
  }

  [Fact(DisplayName = "SaveAsync: it should throw UniqueNameAlreadyUsedException when the unique name is already used.")]
  public async Task Given_UniqueNameConflict_When_SaveAsync_Then_UniqueNameAlreadyUsedException()
  {
    RegionId conflictId = RegionId.NewId();
    _regionQuerier.Setup(x => x.FindIdAsync(_region.UniqueName, _cancellationToken)).ReturnsAsync(conflictId);

    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException>(async () => await _manager.SaveAsync(_region, _cancellationToken));
    Assert.Equal(_region.Id.ToGuid(), exception.EntityId);
    Assert.Equal(conflictId.ToGuid(), exception.ConflictId);
    Assert.Equal(_region.UniqueName.Value, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }
}
