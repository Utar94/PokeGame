using FluentValidation;
using PokeGame.Application.Speciez.Models;

namespace PokeGame.Application.Speciez.Validators;

internal class RegionalNumberValidator : AbstractValidator<RegionalNumberPayload>
{
  public RegionalNumberValidator()
  {
    RuleFor(x => x.Number).GreaterThan(0);
  }
}
