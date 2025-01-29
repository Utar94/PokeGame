using Moq;
using PokeGame.Application.Abilities.Models;

namespace PokeGame.Application.Abilities.Queries;

[Trait(Traits.Category, Categories.Unit)]
public class ReadAbilityQueryHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IAbilityQuerier> _abilityQuerier = new();

  private readonly ReadAbilityQueryHandler _handler;

  private readonly AbilityModel _overgrow = new()
  {
    Id = Guid.NewGuid(),
    UniqueName = "Overgrow"
  };
  private readonly AbilityModel _blaze = new()
  {
    Id = Guid.NewGuid(),
    UniqueName = "Blaze"
  };

  public ReadAbilityQueryHandlerTests()
  {
    _handler = new(_abilityQuerier.Object);

    _abilityQuerier.Setup(x => x.ReadAsync(_overgrow.Id, _cancellationToken)).ReturnsAsync(_overgrow);
    _abilityQuerier.Setup(x => x.ReadAsync(_overgrow.UniqueName, _cancellationToken)).ReturnsAsync(_overgrow);
    _abilityQuerier.Setup(x => x.ReadAsync(_blaze.Id, _cancellationToken)).ReturnsAsync(_blaze);
    _abilityQuerier.Setup(x => x.ReadAsync(_blaze.UniqueName, _cancellationToken)).ReturnsAsync(_blaze);
  }

  [Fact(DisplayName = "It should return null when no ability was found.")]
  public async Task Given_NoneFound_When_Handle_Then_NullReturned()
  {
    ReadAbilityQuery query = new(Id: null, UniqueName: null);
    AbilityModel? ability = await _handler.Handle(query, _cancellationToken);
    Assert.Null(ability);
  }

  [Fact(DisplayName = "It should return the ability found by ID.")]
  public async Task Given_FoundById_When_Handle_Then_Returned()
  {
    ReadAbilityQuery query = new(_overgrow.Id, _overgrow.UniqueName);
    AbilityModel? ability = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(ability);
    Assert.Same(_overgrow, ability);
  }

  [Fact(DisplayName = "It should return the ability found by unique name.")]
  public async Task Given_FoundByUniqueName_When_Handle_Then_Returned()
  {
    ReadAbilityQuery query = new(Guid.Empty, _overgrow.UniqueName);
    AbilityModel? ability = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(ability);
    Assert.Same(_overgrow, ability);
  }

  [Fact(DisplayName = "It should throw TooManyResultsException when more than one ability were found.")]
  public async Task Given_ManyFound_When_Handle_Then_TooManyResultsException()
  {
    ReadAbilityQuery query = new(_overgrow.Id, _blaze.UniqueName);
    var exception = await Assert.ThrowsAsync<TooManyResultsException<AbilityModel>>(async () => await _handler.Handle(query, _cancellationToken));
    Assert.Equal(1, exception.ExpectedCount);
    Assert.Equal(2, exception.ActualCount);
  }
}
