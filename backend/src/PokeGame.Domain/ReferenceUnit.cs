namespace PokeGame.Domain;
public record ReferenceUnit
{
  public string Value { get; }

  public ReferenceUnit(string value)
  {
    Value = value.Trim();
    // TODO(fpion): validate Value
  }

  public static ReferenceUnit? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);
}
