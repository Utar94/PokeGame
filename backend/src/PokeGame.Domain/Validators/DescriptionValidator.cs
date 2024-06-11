using FluentValidation;

namespace PokeGame.Domain.Validators;

public class DescriptionValidator : AbstractValidator<string>
{
  public DescriptionValidator()
  {
    RuleFor(x => x).NotEmpty();
  }
}
