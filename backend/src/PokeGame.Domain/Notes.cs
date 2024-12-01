using FluentValidation;

namespace PokeGame.Domain;

public record Notes
{
  public string Value { get; }

  public Notes(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  public override string ToString() => Value;

  private class Validator : AbstractValidator<Notes>
  {
    public Validator()
    {
      RuleFor(x => x.Value).Notes();
    }
  }
}
