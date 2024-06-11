namespace PokeGame.Domain;
public record DisplayNameUnit
{
  public string Value { get; }

  public DisplayNameUnit(string value)
  {
    Value = value.Trim();
    // TODO(fpion): validate Value
  }

  public static DisplayNameUnit? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);
}
