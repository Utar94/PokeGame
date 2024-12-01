﻿using FluentValidation;
using FluentValidation.Validators;

namespace PokeGame.Domain.Validators;

public class AllowedCharactersValidator<T> : IPropertyValidator<T, string>
{
  public string? AllowedCharacters { get; }
  public string Name { get; } = "AllowedCharactersValidator";

  public AllowedCharactersValidator(string? allowedCharacters)
  {
    AllowedCharacters = allowedCharacters;
  }

  public string GetDefaultMessageTemplate(string errorCode)
  {
    return $"'{{PropertyName}}' may only include the following characters: {AllowedCharacters}";
  }

  public bool IsValid(ValidationContext<T> context, string value)
  {
    return AllowedCharacters == null || value.All(AllowedCharacters.Contains);
  }
}
