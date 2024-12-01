using FluentValidation;

namespace PokeGame.Domain.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class AllowedCharactersValidatorTests
{
  private const string AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

  private readonly ValidationContext<AllowedCharactersValidatorTests> _context;
  private readonly AllowedCharactersValidator<AllowedCharactersValidatorTests> _validator = new(AllowedCharacters);

  public AllowedCharactersValidatorTests()
  {
    _context = new ValidationContext<AllowedCharactersValidatorTests>(this);
  }

  [Theory(DisplayName = "GetDefaultMessageTemplate: it should return the correct message template.")]
  [InlineData(null)]
  [InlineData(AllowedCharacters)]
  public void GetDefaultMessageTemplate_it_should_return_the_correct_message_template(string? allowedCharacters)
  {
    AllowedCharactersValidator<AllowedCharactersValidatorTests> validator = new(allowedCharacters);
    string expected = string.Format("'{{PropertyName}}' may only include the following characters: {0}", allowedCharacters);
    Assert.Equal(expected, validator.GetDefaultMessageTemplate("ErrorCode"));
  }

  [Theory(DisplayName = "IsValid: it should return false when the input value is not valid.")]
  [InlineData("    ")]
  [InlineData("Québ3c!")]
  public void IsValid_it_should_return_false_when_the_input_value_is_not_valid(string value)
  {
    Assert.False(_validator.IsValid(_context, value));
  }

  [Theory(DisplayName = "IsValid: it should return true when the allowed characters are null.")]
  [InlineData("")]
  [InlineData("    ")]
  [InlineData("Québ3c!")]
  public void IsValid_it_should_return_true_when_the_allowed_characters_are_null(string value)
  {
    AllowedCharactersValidator<AllowedCharactersValidatorTests> validator = new(allowedCharacters: null);
    Assert.True(validator.IsValid(_context, value));
  }

  [Theory(DisplayName = "IsValid: it should return true when the input value is valid.")]
  [InlineData("")]
  [InlineData("new-france")]
  public void IsValid_it_should_return_true_when_the_input_value_is_valid(string value)
  {
    Assert.True(_validator.IsValid(_context, value));
  }
}
