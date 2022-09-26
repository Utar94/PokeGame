using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon
{
  public class History
  {
    public History(HistoryPayload payload)
    {
      ArgumentNullException.ThrowIfNull(payload);

      BallId = payload.BallId;
      Level = payload.Level;
      Location = payload.Location;
      MetOn = payload.MetOn;
      TrainerId = payload.TrainerId;
    }

    public Guid BallId { get; }
    public byte Level { get; }
    public string Location { get; }
    public DateTime MetOn { get; }
    public Guid TrainerId { get; }
  }
}
