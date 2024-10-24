using Moq;
using PokeGame.Contracts.Abilities;

namespace PokeGame.Application.Abilities.Queries;

[Trait(Traits.Category, Categories.Unit)]
public class ReadAbilityQueryHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IAbilityQuerier> _abilityQuerier = new();

  private readonly ReadAbilityQueryHandler _handler;

  private readonly AbilityModel _ability = new("Adaptability")
  {
    Id = Guid.NewGuid(),
    Kind = AbilityKind.Adaptability
  };

  public ReadAbilityQueryHandlerTests()
  {
    _handler = new(_abilityQuerier.Object);

    _abilityQuerier.Setup(x => x.ReadAsync(_ability.Id, _cancellationToken)).ReturnsAsync(_ability);
  }

  [Fact(DisplayName = "It should return null when no ability was found.")]
  public async Task It_should_return_null_when_no_ability_was_found()
  {
    ReadAbilityQuery query = new(Guid.NewGuid());
    query.Contextualize();

    Assert.Null(await _handler.Handle(query, _cancellationToken));
  }

  [Fact(DisplayName = "It should return the ability found by ID.")]
  public async Task It_should_return_the_ability_found_by_Id()
  {
    ReadAbilityQuery query = new(_ability.Id);
    query.Contextualize();

    AbilityModel? ability = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(ability);
    Assert.Same(_ability, ability);
  }
}
