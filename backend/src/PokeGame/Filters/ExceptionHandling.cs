﻿using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokeGame.Application;
using PokeGame.Application.Logging;
using PokeGame.Contracts.Errors;
using PokeGame.Domain;

namespace PokeGame.Filters;

internal class ExceptionHandling : ExceptionFilterAttribute
{
  private static readonly JsonSerializerOptions _serializerOptions = new()
  {
    PropertyNameCaseInsensitive = true
  };

  private readonly ILoggingService _loggingService;

  public ExceptionHandling(ILoggingService loggingService)
  {
    _loggingService = loggingService;
  }

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
    else if (context.Exception is DomainException domain)
    {
      context.Result = new BadRequestObjectResult(domain.Error);
      context.ExceptionHandled = true;
    }
    else if (context.Exception is BadRequestException badRequest)
    {
      context.Result = new BadRequestObjectResult(badRequest.Error);
      context.ExceptionHandled = true;
    }
    else if (context.Exception is ConflictException conflict)
    {
      context.Result = new ConflictObjectResult(conflict.Error);
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

    if (context.ExceptionHandled)
    {
      _loggingService.Report(context.Exception);
    }
  }
}
