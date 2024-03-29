﻿using FluentValidation;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities
{
  internal class AbilityValidator : AbstractValidator<Ability>
  {
    public AbilityValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      RuleFor(x => x.Reference)
        .Must(ValidationRules.BeAValidUrl);
    }
  }
}
