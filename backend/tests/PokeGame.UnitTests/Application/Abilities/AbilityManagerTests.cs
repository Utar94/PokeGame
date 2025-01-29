using Moq;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities;

[Trait(Traits.Category, Categories.Unit)]
public class AbilityManagerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IAbilityQuerier> _abilityQuerier = new();
  private readonly Mock<IAbilityRepository> _abilityRepository = new();

  private readonly AbilityManager _manager;

  private readonly Ability _ability = new(new UniqueName("Overgrow"));

  public AbilityManagerTests()
  {
    _manager = new(_abilityQuerier.Object, _abilityRepository.Object);
  }

  [Fact(DisplayName = "SaveAsync: it should not query abilities when the unique did not change.")]
  public async Task Given_NoChange_When_SaveAsync_Then_NotQueried()
  {
    _ability.ClearChanges();

    await _manager.SaveAsync(_ability, _cancellationToken);

    _abilityQuerier.Verify(x => x.FindIdAsync(It.IsAny<UniqueName>(), It.IsAny<CancellationToken>()), Times.Never());
    _abilityRepository.Verify(x => x.SaveAsync(_ability, _cancellationToken), Times.Once());
  }

  [Fact(DisplayName = "SaveAsync: it should save the ability when there is no conflict.")]
  public async Task Given_NoConflict_When_SaveAsync_Then_Abilitiesaved()
  {
    _abilityQuerier.Setup(x => x.FindIdAsync(_ability.UniqueName, _cancellationToken)).ReturnsAsync(_ability.Id);

    await _manager.SaveAsync(_ability, _cancellationToken);

    _abilityQuerier.Verify(x => x.FindIdAsync(_ability.UniqueName, _cancellationToken), Times.Once());
    _abilityRepository.Verify(x => x.SaveAsync(_ability, _cancellationToken), Times.Once());
  }

  [Fact(DisplayName = "SaveAsync: it should throw UniqueNameAlreadyUsedException when the unique name is already used.")]
  public async Task Given_UniqueNameConflict_When_SaveAsync_Then_UniqueNameAlreadyUsedException()
  {
    AbilityId conflictId = AbilityId.NewId();
    _abilityQuerier.Setup(x => x.FindIdAsync(_ability.UniqueName, _cancellationToken)).ReturnsAsync(conflictId);

    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException>(async () => await _manager.SaveAsync(_ability, _cancellationToken));
    Assert.Equal(_ability.Id.ToGuid(), exception.EntityId);
    Assert.Equal(conflictId.ToGuid(), exception.ConflictId);
    Assert.Equal(_ability.UniqueName.Value, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }
}
