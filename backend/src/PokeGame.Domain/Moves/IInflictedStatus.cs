namespace PokeGame.Domain.Moves;

public interface IInflictedStatus
{
  StatusCondition Condition { get; }
  int Chance { get; }
}
