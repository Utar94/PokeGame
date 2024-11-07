using Logitar;
using Moq;
using PokeGame.Domain;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class SaveRegionCommandHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IRegionQuerier> _regionQuerier = new();
  private readonly Mock<IRegionRepository> _regionRepository = new();

  private readonly SaveRegionCommandHandler _handler;

  private readonly Region _region = new(new UniqueName("Kanto"), UserId.NewId());

  public SaveRegionCommandHandlerTests()
  {
    _handler = new(_regionQuerier.Object, _regionRepository.Object);
  }

  [Fact(DisplayName = "It should save a region without changes.")]
  public async Task It_should_save_a_region_without_changes()
  {
    _region.ClearChanges();

    SaveRegionCommand command = new(_region);
    await _handler.Handle(command, _cancellationToken);

    _regionQuerier.Verify(x => x.FindIdAsync(It.IsAny<UniqueName>(), It.IsAny<CancellationToken>()), Times.Never);
    _regionRepository.Verify(x => x.SaveAsync(_region, _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should save the region.")]
  public async Task It_should_save_the_region()
  {
    _regionQuerier.Setup(x => x.FindIdAsync(_region.UniqueName, _cancellationToken)).ReturnsAsync(_region.Id);

    SaveRegionCommand command = new(_region);
    await _handler.Handle(command, _cancellationToken);

    _regionQuerier.Verify(x => x.FindIdAsync(_region.UniqueName, _cancellationToken), Times.Once);
    _regionRepository.Verify(x => x.SaveAsync(_region, _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should throw UniqueNameAlreadyUsedException when the unique name is already used.")]
  public async Task It_should_throw_UniqueNameAlreadyUsedException_when_the_unique_name_is_already_used()
  {
    Region region = new(_region.UniqueName, UserId.NewId());
    _regionQuerier.Setup(x => x.FindIdAsync(_region.UniqueName, _cancellationToken)).ReturnsAsync(region.Id);

    SaveRegionCommand command = new(_region);
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException>(async () => await _handler.Handle(command, _cancellationToken));
    Assert.Equal(typeof(Region).GetNamespaceQualifiedName(), exception.TypeName);
    Assert.Equal(region.UniqueName.Value, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
    Assert.Equal([_region.Id.ToGuid(), region.Id.ToGuid()], exception.ConflictingIds);
  }
}
