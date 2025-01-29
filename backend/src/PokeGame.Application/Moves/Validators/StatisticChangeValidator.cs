using FluentValidation;
using PokeGame.Application.Moves.Models;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Validators;

internal class StatisticChangeValidator : AbstractValidator<StatisticChangeModel>
{
  public StatisticChangeValidator()
  {
    RuleFor(x => x.Statistic).IsInEnum().NotEqual(PokemonStatistic.HP);
    RuleFor(x => x.Stages).InclusiveBetween(Move.MinimumStage, Move.MaximumStage);
  }
}
