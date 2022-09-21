using FluentValidation;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Application.Pokemon
{
  internal class PokemonPositionValidator : AbstractValidator<PokemonPosition>
  {
    public PokemonPositionValidator()
    {
      When(x => x.Box.HasValue, () =>
      {
        RuleFor(x => x.Box!.Value)
          .InclusiveBetween((byte)1, (byte)32);
        RuleFor(x => x.Position)
          .InclusiveBetween((byte)1, (byte)30);
      }).Otherwise(() =>
      {
        RuleFor(x => x.Position)
          .InclusiveBetween((byte)1, (byte)6);
      });
    }
  }
}
