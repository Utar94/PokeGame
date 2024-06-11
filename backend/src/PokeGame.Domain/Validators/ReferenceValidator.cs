using FluentValidation;

namespace PokeGame.Domain.Validators;

public class ReferenceValidator : AbstractValidator<string>
{
  public ReferenceValidator()
  {
    RuleFor(x => x).SetValidator(new UrlValidator());
  }
}
