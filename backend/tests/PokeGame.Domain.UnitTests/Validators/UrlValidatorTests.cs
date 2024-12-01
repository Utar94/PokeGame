using FluentValidation;

namespace PokeGame.Domain.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class UrlValidatorTests
{
  private readonly ValidationContext<UrlValidatorTests> _context;
  private readonly UrlValidator<UrlValidatorTests> _validator = new();

  public UrlValidatorTests()
  {
    _context = new ValidationContext<UrlValidatorTests>(this);
  }

  [Theory(DisplayName = "IsValid: it should return false when the input value is not a valid URL.")]
  [InlineData("")]
  [InlineData("    ")]
  [InlineData("/regions/kanto")]
  [InlineData("ws://bulbapedia.bulbagarden.net/wiki/Kanto")]
  public void IsValid_it_should_return_false_when_the_input_value_is_not_a_valid_Url(string value)
  {
    Assert.False(_validator.IsValid(_context, value));
  }

  [Theory(DisplayName = "IsValid: it should return true when the input value is a valid URL.")]
  [InlineData("https://bulbapedia.bulbagarden.net/wiki/Kanto")]
  [InlineData("HTTP://WWW.POKEMON.COM/REGIONS/KANTO")]
  public void IsValid_it_should_return_true_when_the_input_value_is_valid(string value)
  {
    Assert.True(_validator.IsValid(_context, value));
  }
}
