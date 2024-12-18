using FluentValidation;

namespace PokeGame.Domain;

public record Friendship
{
  public int Value { get; }

  public Friendship()
  {
  }

  public Friendship(int value)
  {
    Value = value;
    new Validator().ValidateAndThrow(this);
  }

  public override string ToString() => Value.ToString();

  private class Validator : AbstractValidator<Friendship>
  {
    public Validator()
    {
      RuleFor(x => x.Value).InclusiveBetween(0, 255);
    }
  }
}
