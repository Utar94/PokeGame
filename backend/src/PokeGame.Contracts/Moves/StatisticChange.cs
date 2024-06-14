namespace PokeGame.Contracts.Moves;

public record StatisticChange
{
  public PokemonStatistic Statistic { get; set; }
  public int Stages { get; set; }
}
