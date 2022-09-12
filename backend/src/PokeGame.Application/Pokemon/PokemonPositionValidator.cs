using FluentValidation;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Application.Pokemon
{
  internal class PokemonPositionValidator : AbstractValidator<PokemonPosition>
  {
    public PokemonPositionValidator()
    {
      When(x => x.Box == null, () =>
      {
        RuleFor(x => x.Position)
          .InclusiveBetween((byte)0, (byte)5);
      }).Otherwise(() =>
      {
        RuleFor(x => x.Box)
          .InclusiveBetween((byte)0, (byte)31);
        RuleFor(x => x.Position)
          .InclusiveBetween((byte)0, (byte)29);
      });
    }
  }
}
