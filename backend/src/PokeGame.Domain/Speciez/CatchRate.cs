using FluentValidation;

namespace PokeGame.Domain.Speciez;

public record CatchRate
{
  public int Value { get; }

  public CatchRate(int value)
  {
    Value = value;
    new Validator().ValidateAndThrow(this);
  }

  public override string ToString() => Value.ToString();

  private class Validator : AbstractValidator<CatchRate>
  {
    public Validator()
    {
      RuleFor(x => x.Value).CatchRate();
    }
  }
}
