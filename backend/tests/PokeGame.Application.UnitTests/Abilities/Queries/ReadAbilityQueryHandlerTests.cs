using Moq;
using PokeGame.Contracts.Abilities;

namespace PokeGame.Application.Abilities.Queries;

[Trait(Traits.Category, Categories.Unit)]
public class ReadAbilityQueryHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IAbilityQuerier> _abilityQuerier = new();

  private readonly ReadAbilityQueryHandler _handler;

  private readonly AbilityModel _adaptability = new("Adaptability")
  {
    Id = Guid.NewGuid()
  };
  private readonly AbilityModel _static = new("Static")
  {
    Id = Guid.NewGuid()
  };

  public ReadAbilityQueryHandlerTests()
  {
    _handler = new(_abilityQuerier.Object);

    _abilityQuerier.Setup(x => x.ReadAsync(_adaptability.Id, _cancellationToken)).ReturnsAsync(_adaptability);
    _abilityQuerier.Setup(x => x.ReadAsync(_adaptability.UniqueName, _cancellationToken)).ReturnsAsync(_adaptability);
    _abilityQuerier.Setup(x => x.ReadAsync(_static.Id, _cancellationToken)).ReturnsAsync(_static);
    _abilityQuerier.Setup(x => x.ReadAsync(_static.UniqueName, _cancellationToken)).ReturnsAsync(_static);
  }

  [Fact(DisplayName = "It should return null when no ability was found.")]
  public async Task It_should_return_null_when_no_ability_was_found()
  {
    ReadAbilityQuery query = new(Guid.NewGuid(), "Overgrow");
    query.Contextualize();

    Assert.Null(await _handler.Handle(query, _cancellationToken));
  }

  [Fact(DisplayName = "It should return the ability found by ID.")]
  public async Task It_should_return_the_ability_found_by_Id()
  {
    ReadAbilityQuery query = new(_adaptability.Id, "Overgrow");
    query.Contextualize();

    AbilityModel? ability = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(ability);
    Assert.Same(_adaptability, ability);
  }

  [Fact(DisplayName = "It should return the ability found by unique name.")]
  public async Task It_should_return_the_ability_found_by_unique_name()
  {
    ReadAbilityQuery query = new(Guid.NewGuid(), _static.UniqueName);
    query.Contextualize();

    AbilityModel? ability = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(ability);
    Assert.Same(_static, ability);
  }

  [Fact(DisplayName = "It should throw TooManyResultsException when more than one abilities were found.")]
  public async Task It_should_throw_TooManyResultsException_when_more_than_one_abilities_were_found()
  {
    ReadAbilityQuery query = new(_adaptability.Id, _static.UniqueName);
    query.Contextualize();

    var exception = await Assert.ThrowsAsync<TooManyResultsException<AbilityModel>>(async () => await _handler.Handle(query, _cancellationToken));
    Assert.Equal(1, exception.ExpectedCount);
    Assert.Equal(2, exception.ActualCount);
  }
}
