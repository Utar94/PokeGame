namespace PokeGame.Domain;
public record DescriptionUnit
{
  public string Value { get; }

  public DescriptionUnit(string value)
  {
    Value = value.Trim();
    // TODO(fpion): validate Value
  }

  public static DescriptionUnit? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);
}
