using FluentValidation;

namespace PokeGame.Domain.Items.Validators;

public class PriceValidator : AbstractValidator<int>
{
  public PriceValidator()
  {
    RuleFor(x => x).GreaterThan(0);
  }
}
