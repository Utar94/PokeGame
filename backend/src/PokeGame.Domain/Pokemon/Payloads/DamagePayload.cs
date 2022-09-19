namespace PokeGame.Domain.Pokemon.Payloads
{
  public class DamagePayload
  {
    public short? Attack { get; set; }
    public bool? IsBurnt { get; set; }
    public bool IsCritical { get; set; }
    public byte? Power { get; set; }
    public double? Random { get; set; }
    public double? STAB { get; set; }
    public Weather Weather { get; set; }
  }
}
