﻿namespace PokeGame.Domain.Pokemon.Payloads
{
  public class TargetPayload
  {
    public Guid Id { get; set; }

    public ushort? Defense { get; set; }
    public double? Effectiveness { get; set; }
    public double? OtherModifiers { get; set; }
  }
}
