using Logitar.Portal.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace PokeGame.Web.Filters
{
  internal class ErrorExceptionFilterAttribute : ExceptionFilterAttribute
  {
    public override void OnException(ExceptionContext context)
    {
      if (context.Exception is ErrorException exception)
      {
        if (exception.Error?.Data != null
          && exception.Error.Data.TryGetValue("Content", out string? json) && json != null
          && exception.Error.Data.TryGetValue("StatusCode", out string? statusCode) && statusCode != null)
        {
          var value = JsonSerializer.Deserialize<object>(json);

          context.ExceptionHandled = true;
          context.Result = new JsonResult(value)
          {
            StatusCode = int.Parse(statusCode)
          };
        }
      }
    }
  }
}
