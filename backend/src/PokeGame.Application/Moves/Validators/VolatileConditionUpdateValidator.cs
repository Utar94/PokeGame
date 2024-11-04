using FluentValidation;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Moves.Validators;

internal class VolatileConditionUpdateValidator : AbstractValidator<VolatileConditionUpdate>
{
  public VolatileConditionUpdateValidator()
  {
    RuleFor(x => x.VolatileCondition).VolatileCondition();
    RuleFor(x => x.Action).IsInEnum();
  }
}
