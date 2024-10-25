namespace PokeGame.Domain;

public record LocaleUnit // TODO(fpion): rename
{
  public const int MaximumLength = 16;

  public string Code { get; }

  public LocaleUnit(string code)
  {
    Code = code;
    // TODO(fpion): implement
  }
}
