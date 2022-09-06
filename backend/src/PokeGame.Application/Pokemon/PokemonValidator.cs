using FluentValidation;

namespace PokeGame.Application.Pokemon
{
  internal class PokemonValidator : AbstractValidator<Domain.Pokemon.Pokemon>
  {
    public PokemonValidator()
    {
      RuleFor(x => x.Level)
        .InclusiveBetween((byte)1, (byte)100);

      RuleFor(x => x.Experience)
        .GreaterThanOrEqualTo(0); // TODO(fpion): calculate max from Level

      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      When(x => x.History != null, () => RuleFor(x => x.History!)
        .SetValidator(x => new HistoryValidator(x)));

      RuleFor(x => x.Reference)
        .Must(ValidationRules.BeAValidUrl);
    }
  }
}
