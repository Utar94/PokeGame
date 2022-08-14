using FluentValidation;

namespace PokeGame.Core.Abilities
{
  internal class AbilityValidator : AbstractValidator<Ability>
  {
    public AbilityValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      RuleFor(x => x.Reference)
        .Must(ValidationRules.BeAValidUrl);
    }
  }
}
