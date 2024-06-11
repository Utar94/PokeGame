using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokeGame.Application;

namespace PokeGame.Filters;

internal class ExceptionHandling : ExceptionFilterAttribute
{
  public override void OnException(ExceptionContext context)
  {
    if (context.Exception is ConflictException conflict)
    {
      context.Result = new ConflictObjectResult(conflict.Error);
      context.ExceptionHandled = true;
    }
    else
    {
      base.OnException(context);
    }
  }
}
