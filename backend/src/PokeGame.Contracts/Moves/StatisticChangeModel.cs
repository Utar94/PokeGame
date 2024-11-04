namespace PokeGame.Contracts.Moves;

public record StatisticChangeModel
{
  public BattleStatistic Statistic { get; set; }
  public int Stages { get; set; }

  public StatisticChangeModel() : this(default, default)
  {
  }

  public StatisticChangeModel(BattleStatistic statistic, int stages)
  {
    Statistic = statistic;
    Stages = stages;
  }
}
