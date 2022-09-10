using FluentValidation;

namespace PokeGame.Application.Pokemon
{
  internal class CurrentHitPointsValidator : AbstractValidator<short>
  {
    public CurrentHitPointsValidator(Domain.Pokemon.Pokemon? pokemon = null)
    {
      RuleFor(x => x)
        .InclusiveBetween((short)0, pokemon?.MaximumHitPoints ?? 0);
    }
  }
}
