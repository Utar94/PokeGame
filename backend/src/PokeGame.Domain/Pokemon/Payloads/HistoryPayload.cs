namespace PokeGame.Domain.Pokemon.Payloads
{
  public class HistoryPayload
  {
    public Guid BallId { get; set; }
    public byte Level { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime MetOn { get; set; }
    public Guid TrainerId { get; set; }
  }
}
