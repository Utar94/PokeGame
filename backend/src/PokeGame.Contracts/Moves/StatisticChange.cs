namespace PokeGame.Contracts.Moves;

public record StatisticChange
{
  public PokemonStatistic Statistic { get; set; }
  public int Stages { get; set; }

  public StatisticChange()
  {
  }

  public StatisticChange(KeyValuePair<string, int> statisticChange)
  {
    Statistic = Enum.Parse<PokemonStatistic>(statisticChange.Key);
    Stages = statisticChange.Value;
  }

  public StatisticChange(PokemonStatistic statistic, int stages)
  {
    Statistic = statistic;
    Stages = stages;
  }
}
