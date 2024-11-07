﻿using FluentValidation;
using FluentValidation.Validators;

namespace PokeGame.Domain.Validators;

internal class AllowedCharactersValidator<T> : IPropertyValidator<T, string>
{
  public string? AllowedCharacters { get; }
  public string Name { get; } = "AllowedCharactersValidator";

  public AllowedCharactersValidator(string? allowedCharacters)
  {
    AllowedCharacters = allowedCharacters;
  }

  public string GetDefaultMessageTemplate(string errorCode)
  {
    return $"'{{PropertyName}}' may only contain the followed characters: {AllowedCharacters}";
  }

  public bool IsValid(ValidationContext<T> context, string value)
  {
    return AllowedCharacters == null || value.All(AllowedCharacters.Contains);
  }
}
