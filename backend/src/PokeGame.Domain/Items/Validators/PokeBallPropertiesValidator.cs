using FluentValidation;
using PokeGame.Contracts.Items.Properties;

namespace PokeGame.Domain.Items.Validators;

public class PokeBallPropertiesValidator : AbstractValidator<IPokeBallProperties>
{
  public PokeBallPropertiesValidator()
  {
    RuleFor(x => x.CatchRateModifier).GreaterThan(0);
  }
}
