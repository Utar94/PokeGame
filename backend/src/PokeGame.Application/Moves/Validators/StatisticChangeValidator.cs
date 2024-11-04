using FluentValidation;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Validators;

internal class StatisticChangeValidator : AbstractValidator<StatisticChangeModel>
{
  public StatisticChangeValidator(bool allowZero = false)
  {
    RuleFor(x => x.Statistic).IsInEnum();

    IRuleBuilderOptions<StatisticChangeModel, int> stages = RuleFor(x => x.Stages).InclusiveBetween(Move.StageMinimumValue, Move.StageMaximumValue);
    if (allowZero)
    {
      stages = stages.NotEqual(0);
    }
  }
}
