using FluentValidation;

namespace PokeGame.Domain.Moves;

public record PowerPoints
{
  public int Value { get; }

  public PowerPoints(int value)
  {
    Value = value;
    new Validator().ValidateAndThrow(this);
  }

  public override string ToString() => Value.ToString();

  private class Validator : AbstractValidator<PowerPoints>
  {
    public Validator()
    {
      RuleFor(x => x.Value).PowerPoints();
    }
  }
}
