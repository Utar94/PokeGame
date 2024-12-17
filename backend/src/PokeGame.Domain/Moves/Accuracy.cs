using FluentValidation;

namespace PokeGame.Domain.Moves;

public record Accuracy
{
  public int Value { get; }

  public Accuracy(int value)
  {
    Value = value;
    new Validator().ValidateAndThrow(this);
  }

  public override string ToString() => Value.ToString();

  private class Validator : AbstractValidator<Accuracy>
  {
    public Validator()
    {
      RuleFor(x => x.Value).InclusiveBetween(1, 100);
    }
  }
}
