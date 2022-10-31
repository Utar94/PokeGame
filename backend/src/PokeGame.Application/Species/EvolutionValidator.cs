using FluentValidation;
using PokeGame.Domain.Species;

namespace PokeGame.Application.Species
{
  internal class EvolutionValidator : AbstractValidator<Evolution>
  {
    public EvolutionValidator()
    {
      When(x => x.Method == EvolutionMethod.Item, () =>
      {
        RuleFor(x => x.HighFriendship)
          .Equal(false);
        RuleFor(x => x.ItemId)
          .NotNull()
          .NotEmpty();
        RuleFor(x => x.Level)
          .Equal((byte)0);
        RuleFor(x => x.Location)
          .Null();
        RuleFor(x => x.MoveId)
          .Null();
        RuleFor(x => x.TimeOfDay)
          .Null();
      });

      When(x => x.Method == EvolutionMethod.LevelUp, () =>
      {
        RuleFor(x => x.Level)
          .LessThanOrEqualTo((byte)100);
        RuleFor(x => x.Location)
          .MaximumLength(100);
      });

      When(x => x.Method == EvolutionMethod.Trade, () =>
      {
        RuleFor(x => x.Gender)
          .Null();
        RuleFor(x => x.HighFriendship)
          .Equal(false);
        RuleFor(x => x.Level)
          .Equal((byte)0);
        RuleFor(x => x.Location)
          .Null();
        RuleFor(x => x.MoveId)
          .Null();
        RuleFor(x => x.RegionId)
          .Null();
        RuleFor(x => x.TimeOfDay)
          .Null();
      });
    }
  }
}
