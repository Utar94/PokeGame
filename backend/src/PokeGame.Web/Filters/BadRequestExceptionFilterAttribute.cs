using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokeGame.Domain.Trainers;

namespace PokeGame.Web.Filters
{
  internal class BadRequestExceptionFilterAttribute : ExceptionFilterAttribute
  {
    public override void OnException(ExceptionContext context)
    {
      if (context.Exception is InsufficientMoneyException)
      {
        Handle(context, new { Code = "InsufficientMoney" });
      }
      else if (context.Exception is InsufficientQuantityException)
      {
        Handle(context, new { Code = "InsufficientQuantity" });
      }
      else if (context.Exception is ItemPriceRequiredException)
      {
        Handle(context, new { Code = "ItemPriceRequired" });
      }
    }

    private static void Handle(ExceptionContext context, object? value = null)
    {
      context.ExceptionHandled = true;
      context.Result = value == null
        ? new BadRequestResult()
        : new BadRequestObjectResult(value);
    }
  }
}
