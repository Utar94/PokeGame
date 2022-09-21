using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PokeGame.Web.Filters
{
  internal class ValidationExceptionFilterAttribute : ExceptionFilterAttribute
  {
    public override void OnException(ExceptionContext context)
    {
      if (context.Exception is ValidationException exception)
      {
        context.ExceptionHandled = true;
        context.Result = new BadRequestObjectResult(exception.Errors);
      }
    }
  }
}
