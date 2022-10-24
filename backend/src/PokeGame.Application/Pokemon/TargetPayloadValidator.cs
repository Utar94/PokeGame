using FluentValidation;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon
{
  internal class TargetPayloadValidator : AbstractValidator<TargetPayload>
  {
    private static readonly HashSet<double> _effectiveness = new(new[] { 0, 0.25, 0.5, 1.0, 2.0, 4.0 });

    public TargetPayloadValidator()
    {
      RuleFor(x => x.Defense)
        .LessThanOrEqualTo((ushort)999);

      RuleFor(x => x.Effectiveness)
        .Must(x => !x.HasValue || _effectiveness.Contains(x.Value));

      RuleFor(x => x.OtherModifiers)
        .GreaterThan(0);
    }
  }
}
