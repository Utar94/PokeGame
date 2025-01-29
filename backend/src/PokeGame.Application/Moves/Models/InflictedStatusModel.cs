using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Models;

public record InflictedStatusModel : IInflictedStatus
{
  public StatusCondition Condition { get; set; }
  public int Chance { get; set; }

  public InflictedStatusModel()
  {
  }

  public InflictedStatusModel(IInflictedStatus status) : this(status.Condition, status.Chance)
  {
  }

  public InflictedStatusModel(StatusCondition condition, int chance)
  {
    Condition = condition;
    Chance = chance;
  }
}
