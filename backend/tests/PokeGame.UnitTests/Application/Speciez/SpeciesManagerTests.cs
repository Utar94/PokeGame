using Moq;
using PokeGame.Domain;
using PokeGame.Domain.Speciez;

namespace PokeGame.Application.Speciez;

[Trait(Traits.Category, Categories.Unit)]
public class SpeciesManagerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<ISpeciesQuerier> _speciesQuerier = new();
  private readonly Mock<ISpeciesRepository> _speciesRepository = new();

  private readonly SpeciesManager _manager;

  private readonly Species _species = new(number: 25, SpeciesCategory.Standard, new UniqueName("Pikachu"), GrowthRate.MediumFast, new Friendship(70), new CatchRate(190));

  public SpeciesManagerTests()
  {
    _manager = new(_speciesQuerier.Object, _speciesRepository.Object);
  }

  [Fact(DisplayName = "SaveAsync: it should not query species when the unique did not change.")]
  public async Task Given_NoChange_When_SaveAsync_Then_NotQueried()
  {
    _species.ClearChanges();

    await _manager.SaveAsync(_species, _cancellationToken);

    _speciesQuerier.Verify(x => x.FindIdAsync(It.IsAny<UniqueName>(), It.IsAny<CancellationToken>()), Times.Never());
    _speciesRepository.Verify(x => x.SaveAsync(_species, _cancellationToken), Times.Once());
  }

  [Fact(DisplayName = "SaveAsync: it should save the species when there is no conflict.")]
  public async Task Given_NoConflict_When_SaveAsync_Then_SpeciesSaved()
  {
    _speciesQuerier.Setup(x => x.FindIdAsync(_species.UniqueName, _cancellationToken)).ReturnsAsync(_species.Id);

    await _manager.SaveAsync(_species, _cancellationToken);

    _speciesQuerier.Verify(x => x.FindIdAsync(_species.UniqueName, _cancellationToken), Times.Once());
    _speciesRepository.Verify(x => x.SaveAsync(_species, _cancellationToken), Times.Once());
  }

  [Fact(DisplayName = "SaveAsync: it should throw UniqueNameAlreadyUsedException when the unique name is already used.")]
  public async Task Given_UniqueNameConflict_When_SaveAsync_Then_UniqueNameAlreadyUsedException()
  {
    SpeciesId conflictId = SpeciesId.NewId();
    _speciesQuerier.Setup(x => x.FindIdAsync(_species.UniqueName, _cancellationToken)).ReturnsAsync(conflictId);

    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException>(async () => await _manager.SaveAsync(_species, _cancellationToken));
    Assert.Equal(_species.Id.ToGuid(), exception.EntityId);
    Assert.Equal(conflictId.ToGuid(), exception.ConflictId);
    Assert.Equal(_species.UniqueName.Value, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }
}
