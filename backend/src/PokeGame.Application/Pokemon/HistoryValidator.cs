using FluentValidation;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Application.Pokemon
{
  internal class HistoryValidator : AbstractValidator<History>
  {
    public HistoryValidator(Domain.Pokemon.Pokemon pokemon)
    {
      ArgumentNullException.ThrowIfNull(pokemon);

      RuleFor(x => x.Level)
        .InclusiveBetween((byte)1, pokemon.Level);

      RuleFor(x => x.Location)
        .NotEmpty()
        .MaximumLength(100);
    }
  }
}
