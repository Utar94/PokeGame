using FluentValidation;
using PokeGame.Domain.Validators;

namespace PokeGame.Domain;
public record NotesUnit
{
  public string Value { get; }

  public NotesUnit(string value)
  {
    Value = value.Trim();
    new NotesValidator().ValidateAndThrow(Value);
  }

  public static NotesUnit? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);
}
