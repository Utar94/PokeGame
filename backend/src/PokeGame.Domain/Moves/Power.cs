using FluentValidation;

namespace PokeGame.Domain.Moves;

public record Power
{
  public int Value { get; }

  public Power(int value)
  {
    Value = value;
    new Validator().ValidateAndThrow(this);
  }

  public override string ToString() => Value.ToString();

  private class Validator : AbstractValidator<Power>
  {
    public Validator()
    {
      RuleFor(x => x.Value).InclusiveBetween(1, 250);
    }
  }
}
