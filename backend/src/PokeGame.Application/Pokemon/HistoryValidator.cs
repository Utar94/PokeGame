using FluentValidation;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Application.Pokemon
{
  internal class HistoryValidator : AbstractValidator<History>
  {
    public HistoryValidator(Domain.Pokemon.Pokemon? pokemon = null)
    {
      RuleFor(x => x.Level)
        .InclusiveBetween((byte)1, pokemon?.Level ?? 100);

      RuleFor(x => x.Location)
        .NotEmpty()
        .MaximumLength(100);
    }
  }
}
