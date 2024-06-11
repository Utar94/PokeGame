using FluentValidation;
using PokeGame.Domain.Validators;

namespace PokeGame.Domain;
public record DisplayNameUnit
{
  public const int MaximumLength = byte.MaxValue;

  public string Value { get; }

  public DisplayNameUnit(string value)
  {
    Value = value.Trim();
    new DisplayNameValidator().ValidateAndThrow(Value);
  }

  public static DisplayNameUnit? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);
}
