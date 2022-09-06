using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon
{
  public class History
  {
    public History(HistoryPayload payload)
      : this(payload?.Level ?? 0, payload?.Location ?? string.Empty, payload?.MetOn ?? default)
    {
      ArgumentNullException.ThrowIfNull(payload);
    }
    public History(byte level, string location, DateTime metOn)
    {
      Level = level;
      Location = location ?? throw new ArgumentNullException(nameof(location));
      MetOn = metOn;
    }

    public byte Level { get; }
    public string Location { get; }
    public DateTime MetOn { get; }
  }
}
