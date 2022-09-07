using FluentValidation;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Application.Pokemon
{
  internal class PokemonValidator : AbstractValidator<Domain.Pokemon.Pokemon>
  {
    public PokemonValidator()
    {
      RuleFor(x => x.Level)
        .InclusiveBetween((byte)1, (byte)100);

      RuleFor(x => x.Experience)
        .GreaterThanOrEqualTo(x => ExperienceTable.GetTotalExperience(x.LevelingRate, x.Level))
        .LessThan(x => ExperienceTable.GetTotalExperience(x.LevelingRate, x.Level < 100 ? (x.Level + 1) : 100));

      RuleFor(x => x.GenderRatio)
        .InclusiveBetween(0.0, 100.0);

      When(x => x.GenderRatio == null, () =>
        RuleFor(x => x.Gender)
          .Equal(PokemonGender.Unknown)
      );
      When(x => x.GenderRatio != null, () =>
      {
        When(x => x.GenderRatio!.Value == 0.0, () =>
          RuleFor(x => x.Gender)
            .Equal(PokemonGender.Female)
        );
        When(x => x.GenderRatio!.Value == 100.0, () =>
          RuleFor(x => x.Gender)
            .Equal(PokemonGender.Male)
        );
        When(x => x.GenderRatio!.Value > 0.0 && x.GenderRatio!.Value < 100.0, () =>
          RuleFor(x => x.Gender)
            .NotEqual(PokemonGender.Unknown)
        );
      });

      RuleFor(x => x.Nature)
        .NotNull();

      RuleFor(x => x.SpeciesName)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Surname)
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      RuleForEach(x => x.BaseStatistics.Values)
        .GreaterThan((byte)0);

      RuleForEach(x => x.IndividualValues.Values)
        .InclusiveBetween((byte)0, (byte)31);

      RuleFor(x => x.EffortValues)
        .Must(x => x.Values.Sum(y => y) <= 510);

      RuleForEach(x => x.Statistics.Values)
        .InclusiveBetween((short)0, (short)999);

      RuleFor(x => x.Moves)
        .Must(moves => moves.GroupBy(y => y.MoveId).Count() == moves.Count
          && moves.GroupBy(y => y.Position).Count() == moves.Count);
      RuleForEach(x => x.Moves)
        .SetValidator(new PokemonMoveValidator());

      When(x => x.History == null, () =>
      {
        RuleFor(x => x.OriginalTrainer)
          .Null();
        RuleFor(x => x.Position)
          .Null();
        RuleFor(x => x.Box)
          .Null();
      });
      When(x => x.History != null, () =>
      {
        RuleFor(x => x.History!)
          .SetValidator(x => new HistoryValidator(x));
        RuleFor(x => x.OriginalTrainer)
          .NotNull();
        RuleFor(x => x.Position)
          .NotNull();

        When(x => x.Box == null, () =>
          RuleFor(x => x.Position)
            .InclusiveBetween((byte)0, (byte)5)
        );
        When(x => x.Box != null, () =>
          RuleFor(x => x.Position)
            .InclusiveBetween((byte)0, (byte)29)
        );
      });

      RuleFor(x => x.Reference)
        .Must(ValidationRules.BeAValidUrl);
    }
  }
}
