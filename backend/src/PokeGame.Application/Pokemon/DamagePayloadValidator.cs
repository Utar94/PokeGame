using FluentValidation;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon
{
  internal class DamagePayloadValidator : AbstractValidator<DamagePayload>
  {
    private static readonly HashSet<double> _stab = new(new[] { 1.0, 1.5, 2.0 });

    public DamagePayloadValidator()
    {
      RuleFor(x => x.Attack)
       .LessThanOrEqualTo((ushort)999);

      RuleFor(x => x.Random)
        .InclusiveBetween((double)0.85, (double)1.0);

      RuleFor(x => x.STAB)
        .Must(x => !x.HasValue || _stab.Contains(x.Value));
    }
  }
}
