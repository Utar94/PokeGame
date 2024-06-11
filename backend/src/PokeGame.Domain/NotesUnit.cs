namespace PokeGame.Domain;
public record NotesUnit
{
  public string Value { get; }

  public NotesUnit(string value)
  {
    Value = value.Trim();
    // TODO(fpion): validate Value
  }

  public static NotesUnit? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);
}
