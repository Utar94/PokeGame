using FluentValidation;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Application.Moves
{
  internal class MoveValidator : AbstractValidator<Move>
  {
    public MoveValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      When(x => x.Category == MoveCategory.Status, () => RuleFor(x => x.Power).Null());

      RuleFor(x => x.Accuracy)
        .LessThanOrEqualTo((byte)100)
        .Must(x => x == null || x % 5 == 0);

      RuleFor(x => x.Power)
        .Must(x => x == null || x % 5 == 0);

      RuleFor(x => x.PowerPoints)
        .InclusiveBetween((byte)5, (byte)40)
        .Must(x => x % 5 == 0);

      When(x => x.StatusCondition == null, () =>
      {
        RuleFor(x => x.StatusChance)
          .Null();
      }).Otherwise(() =>
      {
        RuleFor(x => x.StatusChance)
          .InclusiveBetween((byte)1, (byte)100);
      });

      RuleFor(x => x.StatisticStages)
        .Must(x => !x.ContainsKey(Statistic.HP));
      RuleForEach(x => x.StatisticStages.Values)
        .InclusiveBetween((short)-6, (short)6);

      RuleFor(x => x.AccuracyStage)
        .InclusiveBetween((short)-6, (short)6);

      RuleFor(x => x.EvasionStage)
        .InclusiveBetween((short)-6, (short)6);

      RuleFor(x => x.Reference)
        .Must(ValidationRules.BeAValidUrl);
    }
  }
}
