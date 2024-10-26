using PokeGame.Contracts.Abilities;

namespace PokeGame.Domain.Abilities;

[Trait(Traits.Category, Categories.Unit)]
public class AbilityTests
{
  private readonly Ability _ability = new(new Name("Adaptability"), UserId.NewId());

  [Fact(DisplayName = "It should throw ArgumentOutOfRangeException when setting an undefined value.")]
  public void It_should_throw_ArgumentOutOfRangeException_when_setting_an_undefined_value()
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _ability.Kind = (AbilityKind)(-1));
    Assert.Equal("Kind", exception.ParamName);
  }
}
