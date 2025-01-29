using PokeGame.Domain;

namespace PokeGame.Application.Moves.Models;

public record StatisticChangeModel
{
  public PokemonStatistic Statistic { get; set; }
  public int Stages { get; set; }
}
