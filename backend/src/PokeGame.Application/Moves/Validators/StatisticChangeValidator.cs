using FluentValidation;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;

namespace PokeGame.Application.Moves.Validators;

internal class StatisticChangeValidator : AbstractValidator<StatisticChange>
{
  public StatisticChangeValidator()
  {
    RuleFor(x => x.Statistic).IsInEnum().NotEqual(PokemonStatistic.HP);
    RuleFor(x => x.Stages).InclusiveBetween(-6, 6).NotEqual(0);
  }
}
