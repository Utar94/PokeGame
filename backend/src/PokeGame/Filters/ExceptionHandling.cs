using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokeGame.Contracts.Errors;
using System.Text.Json;

namespace PokeGame.Filters;

internal class ExceptionHandling : ExceptionFilterAttribute
{
  private static readonly JsonSerializerOptions _serializerOptions = new()
  {
    PropertyNameCaseInsensitive = true
  };

  public override void OnException(ExceptionContext context)
  {
    if (context.Exception is ValidationException validation)
    {
      ValidationError error = new();
      foreach (ValidationFailure failure in validation.Errors)
      {
        error.Add(new PropertyError(failure.ErrorCode, failure.ErrorMessage, failure.AttemptedValue, failure.PropertyName));
      }
      context.Result = new BadRequestObjectResult(error);
      context.ExceptionHandled = true;
    }
    else if (context.Exception is NotImplementedException)
    {
      context.Result = new StatusCodeResult(StatusCodes.Status501NotImplemented);
      context.ExceptionHandled = true;
    }
    else
    {
      base.OnException(context);
    }
  }
}
