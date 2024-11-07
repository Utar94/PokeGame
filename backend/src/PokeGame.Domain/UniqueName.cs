using FluentValidation;
using PokeGame.Domain.Validators;

namespace PokeGame.Domain;

public record UniqueName
{
  public const int MaximumLength = byte.MaxValue;

  public string Value { get; }

  public UniqueName(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  public static UniqueName? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  public override string ToString() => Value;

  private class Validator : AbstractValidator<UniqueName>
  {
    public Validator()
    {
      RuleFor(x => x.Value).UniqueName();
    }
  }
}
