using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using PokeGame.Domain.Validators;

namespace PokeGame.Domain;
public record UniqueNameUnit
{
  public const int MaximumLength = byte.MaxValue;

  public string Value { get; }

  public UniqueNameUnit(IUniqueNameSettings uniqueNameSettings, string value)
  {
    Value = value.Trim();
    new UniqueNameValidator(uniqueNameSettings).ValidateAndThrow(Value);
  }

  public static UniqueNameUnit? TryCreate(IUniqueNameSettings uniqueNameSettings, string? value)
  {
    return string.IsNullOrWhiteSpace(value) ? null : new(uniqueNameSettings, value);
  }
}
