namespace PokeGame.Contracts.Moves;

public record StatisticChangeModel
{
  public BattleStatistic Statistic { get; set; }
  public int Stages { get; set; }
}
