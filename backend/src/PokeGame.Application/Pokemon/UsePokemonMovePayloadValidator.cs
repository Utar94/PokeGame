using FluentValidation;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon
{
  internal class UsePokemonMovePayloadValidator : AbstractValidator<UsePokemonMovePayload>
  {
    public UsePokemonMovePayloadValidator()
    {
      When((payload, context) => context.GetMoveCategory() == MoveCategory.Status, () =>
      {
        RuleFor(x => x.Damage)
          .Null();
      }).Otherwise(() =>
      {
        RuleFor(x => x.Damage!)
          .NotNull()
          .SetValidator(new DamagePayloadValidator());
      });

      RuleFor(x => x.Targets)
        .NotNull()
        .NotEmpty();
      RuleForEach(x => x.Targets)
        .SetValidator(new TargetPayloadValidator());
    }
  }
}
