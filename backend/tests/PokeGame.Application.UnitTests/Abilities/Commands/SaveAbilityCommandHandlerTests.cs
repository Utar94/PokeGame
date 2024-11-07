using Logitar;
using Moq;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class SaveAbilityCommandHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IAbilityQuerier> _abilityQuerier = new();
  private readonly Mock<IAbilityRepository> _abilityRepository = new();

  private readonly SaveAbilityCommandHandler _handler;

  private readonly Ability _ability = new(new UniqueName("Kanto"), UserId.NewId());

  public SaveAbilityCommandHandlerTests()
  {
    _handler = new(_abilityQuerier.Object, _abilityRepository.Object);
  }

  [Fact(DisplayName = "It should save an ability without changes.")]
  public async Task It_should_save_an_ability_without_changes()
  {
    _ability.ClearChanges();

    SaveAbilityCommand command = new(_ability);
    await _handler.Handle(command, _cancellationToken);

    _abilityQuerier.Verify(x => x.FindIdAsync(It.IsAny<UniqueName>(), It.IsAny<CancellationToken>()), Times.Never);
    _abilityRepository.Verify(x => x.SaveAsync(_ability, _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should save the ability.")]
  public async Task It_should_save_the_ability()
  {
    _abilityQuerier.Setup(x => x.FindIdAsync(_ability.UniqueName, _cancellationToken)).ReturnsAsync(_ability.Id);

    SaveAbilityCommand command = new(_ability);
    await _handler.Handle(command, _cancellationToken);

    _abilityQuerier.Verify(x => x.FindIdAsync(_ability.UniqueName, _cancellationToken), Times.Once);
    _abilityRepository.Verify(x => x.SaveAsync(_ability, _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should throw UniqueNameAlreadyUsedException when the unique name is already used.")]
  public async Task It_should_throw_UniqueNameAlreadyUsedException_when_the_unique_name_is_already_used()
  {
    Ability ability = new(_ability.UniqueName, UserId.NewId());
    _abilityQuerier.Setup(x => x.FindIdAsync(_ability.UniqueName, _cancellationToken)).ReturnsAsync(ability.Id);

    SaveAbilityCommand command = new(_ability);
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException>(async () => await _handler.Handle(command, _cancellationToken));
    Assert.Equal(typeof(Ability).GetNamespaceQualifiedName(), exception.TypeName);
    Assert.Equal(ability.UniqueName.Value, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
    Assert.Equal([_ability.Id.ToGuid(), ability.Id.ToGuid()], exception.ConflictingIds);
  }
}
