﻿using FluentValidation;
using PokeGame.Domain.Validators;

namespace PokeGame.Domain;

public record Name // TODO(fpion): remove this class
{
  public const int MaximumLength = byte.MaxValue;

  public string Value { get; }

  public Name(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  public static Name? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  public override string ToString() => Value;

  private class Validator : AbstractValidator<Name>
  {
    public Validator()
    {
      RuleFor(x => x.Value).Name();
    }
  }
}
