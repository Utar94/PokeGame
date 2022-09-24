using PokeGame.Domain.Pokemon;

namespace PokeGame.Domain.Species.Payloads
{
  public class StatisticValuePayload
  {
    public StatisticValuePayload(KeyValuePair<Statistic, byte>? pair = null)
    {
      if (pair.HasValue)
      {
        Statistic = pair.Value.Key;
        Value = pair.Value.Value;
      }
    }

    public Statistic Statistic { get; set; }
    public byte Value { get; set; }
  }
}
