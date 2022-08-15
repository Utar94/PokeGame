namespace PokeGame.Core.Moves.Payloads
{
  public abstract class SaveMovePayload
  {
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public double? Accuracy { get; set; }
    public int? Power { get; set; }
    public int PowerPoints { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
