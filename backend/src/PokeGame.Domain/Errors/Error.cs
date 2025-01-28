namespace PokeGame.Domain.Errors;

public record Error
{
  public string Code { get; }
  public string Message { get; }
  public IReadOnlyDictionary<string, object?> Data { get; }

  public Error(string code, string message, IReadOnlyDictionary<string, object?>? data = null)
  {
    Code = code;
    Message = message;
    Data = data ?? new Dictionary<string, object?>().AsReadOnly();
  }
}
