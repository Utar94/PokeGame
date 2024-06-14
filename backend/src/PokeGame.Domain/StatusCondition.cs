namespace PokeGame.Domain;

public readonly struct StatusCondition
{
  public static readonly IImmutableSet<string> NonVolatileConditions = ImmutableHashSet.Create(["Burn", "Freeze", "Paralysis", "Poison", "Sleep"]);

  private readonly string? _value;
  public string Value => _value ?? string.Empty;

  public bool IsVolatile => NonVolatileConditions.Contains(Value);

  public StatusCondition(string value)
  {
    _value = Format(value);
  }

  public static string Format(string value)
  {
    value = value.Trim();

    foreach (string condition in NonVolatileConditions)
    {
      if (condition.Equals(value, StringComparison.InvariantCultureIgnoreCase))
      {
        return condition;
      }
    }

    return value;
  }

  public static bool operator ==(StatusCondition left, StatusCondition right) => left.Equals(right);
  public static bool operator !=(StatusCondition left, StatusCondition right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is StatusCondition condition && condition.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
