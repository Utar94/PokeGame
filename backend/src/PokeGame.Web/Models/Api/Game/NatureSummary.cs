using PokeGame.Domain;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Web.Models.Api.Game
{
  public class NatureSummary
  {
    public NatureSummary(string name)
    {
      var nature = Nature.GetNature(name);

      Name = nature.Name;

      IncreasedStatistic = nature.IncreasedStatistic;
      DecreasedStatistic = nature.DecreasedStatistic;

      FavoriteFlavor = nature.FavoriteFlavor;
    }

    public string Name { get; set; }

    public Statistic? IncreasedStatistic { get; set; }
    public Statistic? DecreasedStatistic { get; set; }

    public Flavor? FavoriteFlavor { get; set; }
  }
}
