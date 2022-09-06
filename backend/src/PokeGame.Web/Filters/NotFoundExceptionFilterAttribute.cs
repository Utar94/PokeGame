using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokeGame.Application;
using PokeGame.Application.Inventories;
using PokeGame.Application.Species;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Web.Filters
{
  internal class NotFoundExceptionFilterAttribute : ExceptionFilterAttribute
  {
    public override void OnException(ExceptionContext context)
    {
      if (context.Exception is EntityNotFoundException entityNotFound)
      {
        if (entityNotFound.Data.Contains("ParamName"))
        {
          Handle(context, new { Field = entityNotFound.Data["ParamName"] });
        }
        else
        {
          Handle(context);
        }
      }
      else if (context.Exception is AbilitiesNotFoundException abilitiesNotFound)
      {
        Handle(context, new { Ids = abilitiesNotFound.Data["Ids"] });
      }
      else if (context.Exception is InventoryNotFoundException inventoryNotFound)
      {
        Handle(context, new
        {
          ItemId = inventoryNotFound.Data["ItemId"],
          TrainerId = inventoryNotFound.Data["TrainerId"]
        });
      }
      else if (context.Exception is NatureNotFoundException natureNotFound)
      {
        Handle(context, new { Field = natureNotFound.Data["ParamName"] });
      }
    }

    private static void Handle(ExceptionContext context, object? value = null)
    {
      context.ExceptionHandled = true;
      context.Result = value == null
        ? new NotFoundResult()
        : new NotFoundObjectResult(value);
    }
  }
}
