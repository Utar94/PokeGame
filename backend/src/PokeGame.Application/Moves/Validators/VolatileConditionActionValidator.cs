using FluentValidation;
using PokeGame.Application.Moves.Models;
using PokeGame.Domain;

namespace PokeGame.Application.Moves.Validators;

internal class VolatileConditionActionValidator : AbstractValidator<VolatileConditionAction>
{
  public VolatileConditionActionValidator()
  {
    RuleFor(x => x.Value).VolatileCondition();
    RuleFor(x => x.Action).IsInEnum();
  }
}
