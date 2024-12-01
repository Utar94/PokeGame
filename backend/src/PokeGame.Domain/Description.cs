using FluentValidation;

namespace PokeGame.Domain;

public record Description
{
  public string Value { get; }

  public Description(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  public override string ToString() => Value;

  private class Validator : AbstractValidator<Description>
  {
    public Validator()
    {
      RuleFor(x => x.Value).Description();
    }
  }
}
