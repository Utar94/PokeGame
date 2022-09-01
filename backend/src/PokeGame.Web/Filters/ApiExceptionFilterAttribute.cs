using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokeGame.Core;

namespace PokeGame.Web.Filters
{
  internal class ApiExceptionFilterAttribute : ExceptionFilterAttribute
  {
    public override void OnException(ExceptionContext context)
    {
      if (context.Exception is ApiException exception)
      {
        context.ExceptionHandled = true;
        context.Result = exception.Value == null
          ? new StatusCodeResult((int)exception.StatusCode)
          : new JsonResult(exception.Value) { StatusCode = (int)exception.StatusCode };
      }
    }
  }
}
