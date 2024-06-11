using FluentValidation;

namespace PokeGame.Domain.Validators;

public class AllowedCharactersValidator : AbstractValidator<string>
{
  public AllowedCharactersValidator(string? allowedCharacters)
  {
    if (allowedCharacters != null)
    {
      RuleFor(x => x).Must(value => value.All(allowedCharacters.Contains))
        .WithErrorCode(nameof(AllowedCharactersValidator))
        .WithMessage($"'{{PropertyName}}' may only include the following characters: '{allowedCharacters}'.");
    }
  }
}
