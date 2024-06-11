namespace PokeGame.Domain;
public record UniqueNameUnit
{
  public const int MaximumLength = byte.MaxValue;

  public string Value { get; }

  public UniqueNameUnit(string value)
  {
    Value = value.Trim();
    // TODO(fpion): validate Value
  }

  public static UniqueNameUnit? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);
}
