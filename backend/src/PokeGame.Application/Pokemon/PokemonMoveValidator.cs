using FluentValidation;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Application.Pokemon
{
  internal class PokemonMoveValidator : AbstractValidator<PokemonMove>
  {
    public PokemonMoveValidator()
    {
      RuleFor(x => x.Position)
        .InclusiveBetween((byte)0, (byte)3);
    }
  }
}
