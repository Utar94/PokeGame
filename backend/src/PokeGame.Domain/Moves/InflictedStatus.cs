using FluentValidation;

namespace PokeGame.Domain.Moves;

public record InflictedStatus
{
  public StatusCondition Condition { get; }
  public int Chance { get; }

  public InflictedStatus(StatusCondition condition, int chance)
  {
    Condition = condition;
    Chance = chance;
    new Validator().ValidateAndThrow(this);
  }

  private class Validator : AbstractValidator<InflictedStatus>
  {
    public Validator()
    {
      RuleFor(x => x.Condition).IsInEnum();
      RuleFor(x => x.Chance).InclusiveBetween(1, 100);
    }
  }
}
