using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Domain.Pokemon.Payloads
{
  public class ExperienceGainPayload
  {
    public uint Experience { get; set; }
    public byte Friendship { get; set; }
    public IEnumerable<StatisticValuePayload>? EffortValues { get; set; }
  }
}
