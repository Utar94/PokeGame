using FluentValidation;

namespace PokeGame.Domain.Validators;

public class UrlValidator : AbstractValidator<string>
{
  public const int MaximumLength = 2048;

  public UrlValidator()
  {
    RuleFor(x => x).NotEmpty().MaximumLength(MaximumLength)
      .Must(value => Uri.IsWellFormedUriString(value, UriKind.Absolute))
        .WithErrorCode(nameof(UrlValidator))
        .WithMessage("'{PropertyName}' must be a valid Uniform Resource Locator. See https://en.wikipedia.org/wiki/URL for more info.");
  }
}
