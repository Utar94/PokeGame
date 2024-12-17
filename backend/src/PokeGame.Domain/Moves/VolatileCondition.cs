using FluentValidation;

namespace PokeGame.Domain.Moves;

public record VolatileCondition
{
  public const int MaximumLength = byte.MaxValue;

  public string Value { get; }

  public VolatileCondition(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  public static VolatileCondition? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  public override string ToString() => Value;

  private class Validator : AbstractValidator<VolatileCondition>
  {
    public Validator()
    {
      RuleFor(x => x.Value).VolatileCondition();
    }
  }
}
