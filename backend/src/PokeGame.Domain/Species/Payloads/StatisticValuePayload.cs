using PokeGame.Domain.Pokemon;

namespace PokeGame.Domain.Species.Payloads
{
  public class StatisticValuePayload
  {
    public Statistic Statistic { get; set; }
    public byte Value { get; set; }
  }
}
