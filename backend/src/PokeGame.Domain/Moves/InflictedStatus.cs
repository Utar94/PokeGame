using FluentValidation;

namespace PokeGame.Domain.Moves;

public record InflictedStatus : IInflictedStatus
{
  public StatusCondition Condition { get; }
  public int Chance { get; }

  [JsonConstructor]
  public InflictedStatus(StatusCondition condition, int chance)
  {
    Condition = condition;
    Chance = chance;
    new InflictedStatusValidator().ValidateAndThrow(this);
  }

  public InflictedStatus(IInflictedStatus status) : this(status.Condition, status.Chance)
  {
  }
}
