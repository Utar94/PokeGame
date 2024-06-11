using FluentValidation;
using FluentValidation.Results;
using PokeGame.Contracts.Errors;

namespace PokeGame.Extensions;

internal static class FluentValidationExtensions
{
  public static ValidationError ToValidationError(this ValidationException exception)
  {
    ValidationError error = new();
    foreach (ValidationFailure failure in exception.Errors)
    {
      error.Add(new PropertyError(failure.ErrorCode, failure.ErrorMessage, failure.PropertyName, failure.AttemptedValue));
    }
    return error;
  }
}
