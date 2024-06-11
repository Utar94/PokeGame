using FluentValidation;

namespace PokeGame.Domain.Validators;

public class DisplayNameValidator : AbstractValidator<string>
{
  public DisplayNameValidator()
  {
    RuleFor(x => x).NotEmpty().MaximumLength(DisplayNameUnit.MaximumLength);
  }
}
