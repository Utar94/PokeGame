namespace PokeGame.Domain.Trainers.Payloads
{
  public abstract class SaveTrainerPayload
  {
    public Guid? UserId { get; set; }

    public int Money { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Picture { get; set; }
    public string? Reference { get; set; }
  }
}
