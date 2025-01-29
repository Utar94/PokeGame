using PokeGame.Domain;

namespace PokeGame.Application.Moves.Models;

public record StatisticChangeModel
{
  public PokemonStatistic Statistic { get; set; }
  public int Stages { get; set; }

  public StatisticChangeModel()
  {
  }

  public StatisticChangeModel(KeyValuePair<PokemonStatistic, int> change) : this(change.Key, change.Value)
  {
  }

  public StatisticChangeModel(PokemonStatistic statistic, int stages)
  {
    Statistic = statistic;
    Stages = stages;
  }
}
