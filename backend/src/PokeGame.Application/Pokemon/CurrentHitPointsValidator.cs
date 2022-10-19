using FluentValidation;

namespace PokeGame.Application.Pokemon
{
  internal class CurrentHitPointsValidator : AbstractValidator<ushort>
  {
    public CurrentHitPointsValidator(Domain.Pokemon.Pokemon? pokemon = null)
    {
      RuleFor(x => x)
        .LessThanOrEqualTo(pokemon?.MaximumHitPoints ?? 0);
    }
  }
}
