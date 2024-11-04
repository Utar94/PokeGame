namespace PokeGame.Contracts.Moves;

public interface IInflictedCondition
{
  StatusCondition Condition { get; }
  int Chance { get; }
}
