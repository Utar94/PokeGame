using PokeGame.Domain.Pokemon;

namespace PokeGame.Domain.Moves.Payloads
{
  public class StatisticStagePayload
  {
    public Statistic Statistic { get; set; }
    public short Value { get; set; }
  }
}
