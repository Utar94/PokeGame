using FluentValidation;
using PokeGame.Application.Speciez.Models;

namespace PokeGame.Application.Speciez.Validators;

internal class RegionalNumberUpdateValidator : AbstractValidator<RegionalNumberUpdate>
{
  public RegionalNumberUpdateValidator()
  {
    When(x => x.Number.HasValue, () => RuleFor(x => x.Number!.Value).GreaterThan(0));
  }
}
