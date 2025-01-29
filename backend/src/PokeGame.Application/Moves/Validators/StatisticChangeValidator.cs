using FluentValidation;
using PokeGame.Application.Moves.Models;
using PokeGame.Domain;

namespace PokeGame.Application.Moves.Validators;

internal class StatisticChangeValidator : AbstractValidator<StatisticChangeModel>
{
  public StatisticChangeValidator()
  {
    RuleFor(x => x.Statistic).IsInEnum().NotEqual(PokemonStatistic.HP);
    RuleFor(x => x.Stages).InclusiveBetween(-6, 6).NotEqual(0); // TODO(fpion): refactor
  }
}
