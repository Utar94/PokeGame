﻿using Logitar.EventSourcing;

namespace PokeGame.Domain;

public readonly struct UserId
{
  public ActorId ActorId { get; }
  public string Value => ActorId.Value;

  public UserId(ActorId actorId)
  {
    ActorId = actorId;
  }
  public UserId(Guid value)
  {
    ActorId = new(value);
  }
  public UserId(string value)
  {
    ActorId = new(value);
  }

  public static bool operator ==(UserId left, UserId right) => left.Equals(right);
  public static bool operator !=(UserId left, UserId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is UserId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
