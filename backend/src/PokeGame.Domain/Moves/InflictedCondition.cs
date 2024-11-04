using FluentValidation;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;

namespace PokeGame.Domain.Moves;

public record InflictedCondition : IInflictedCondition
{
  public StatusCondition Condition { get; }
  public int Chance { get; }

  public InflictedCondition(IInflictedCondition status) : this(status.Condition, status.Chance)
  {
  }

  public InflictedCondition(StatusCondition condition, int chance)
  {
    Condition = condition;
    Chance = chance;
    new InflictedConditionValidator().ValidateAndThrow(this);
  }
}
