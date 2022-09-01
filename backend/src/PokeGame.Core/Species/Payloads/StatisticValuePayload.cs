using PokeGame.Core.Pokemon;

namespace PokeGame.Core.Species.Payloads
{
  public class StatisticValuePayload
  {
    public Statistic Statistic { get; set; }
    public byte Value { get; set; }
  }
}
