using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Models;

public record InflictedStatusModel : IInflictedStatus
{
  public StatusCondition Condition { get; set; }
  public int Chance { get; set; }
}
