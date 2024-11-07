using Moq;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class DeleteAbilityCommandHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IAbilityQuerier> _abilityQuerier = new();
  private readonly Mock<IAbilityRepository> _abilityRepository = new();

  private readonly DeleteAbilityCommandHandler _handler;

  private readonly UserId _userId = UserId.NewId();
  private readonly Ability _ability;

  public DeleteAbilityCommandHandlerTests()
  {
    _handler = new(_abilityQuerier.Object, _abilityRepository.Object);

    _ability = new(new UniqueName("Adaptability"), _userId);
    _abilityRepository.Setup(x => x.LoadAsync(_ability.Id, _cancellationToken)).ReturnsAsync(_ability);
  }

  [Fact(DisplayName = "It should delete an existing ability.")]
  public async Task It_should_delete_an_existing_ability()
  {
    DeleteAbilityCommand command = new(_ability.Id.ToGuid());
    command.Contextualize();

    AbilityModel model = new();
    _abilityQuerier.Setup(x => x.ReadAsync(_ability, _cancellationToken)).ReturnsAsync(model);

    AbilityModel? result = await _handler.Handle(command, _cancellationToken);
    Assert.NotNull(result);
    Assert.Same(model, result);

    _abilityRepository.Verify(x => x.SaveAsync(It.Is<Ability>(y => y.Equals(_ability) && y.IsDeleted), _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should return null when the ability could not be found.")]
  public async Task It_should_return_null_when_the_ability_could_not_be_found()
  {
    DeleteAbilityCommand command = new(Guid.NewGuid());
    command.Contextualize();

    Assert.Null(await _handler.Handle(command, _cancellationToken));
  }
}
