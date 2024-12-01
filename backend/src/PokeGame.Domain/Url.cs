using FluentValidation;

namespace PokeGame.Domain;

public record Url
{
  public const int MaximumLength = 2048;

  public string Value { get; }

  public Url(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  public override string ToString() => Value;

  private class Validator : AbstractValidator<Url>
  {
    public Validator()
    {
      RuleFor(x => x.Value).Url();
    }
  }
}
