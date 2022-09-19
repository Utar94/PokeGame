using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokeGame.Application;
using PokeGame.Application.Inventories;
using PokeGame.Application.Pokemon;
using PokeGame.Application.Species;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Trainers;

namespace PokeGame.Web.Filters
{
  internal class NotFoundExceptionFilterAttribute : ExceptionFilterAttribute
  {
    private static readonly Dictionary<Type, Func<ExceptionContext, ActionResult>> _handlers = new()
    {
      [typeof(AbilitiesNotFoundException)] = HandleAbilitiesNotFound,
      [typeof(EntityNotFoundException)] = HandleEntityNotFound,
      [typeof(InventoryNotFoundException)] = HandleInventoryNotFound,
      [typeof(MovesNotFoundException)] = HandleMovesNotFound,
      [typeof(NatureNotFoundException)] = HandleNatureNotFound,
      [typeof(PokedexEntryNotFoundException)] = HandlePokedexEntryNotFound,
      [typeof(PokemonMoveNotFoundException)] = HandlePokemonMoveNotFound,
      [typeof(PokemonNotFoundException)] = HandlePokemonNotFound
    };

    public override void OnException(ExceptionContext context)
    {
      if (_handlers.TryGetValue(context.Exception.GetType(), out Func<ExceptionContext, ActionResult>? handler))
      {
        context.Result = handler(context);
        context.ExceptionHandled = true;
      }
    }

    private static ActionResult HandleAbilitiesNotFound(ExceptionContext context)
      => new NotFoundObjectResult(new { Ids = context.Exception.Data["Ids"] });

    private static ActionResult HandleEntityNotFound(ExceptionContext context)
      => context.Exception.Data.Contains("ParamName")
        ? new NotFoundObjectResult(new { Field = context.Exception.Data["ParamName"] })
        : new NotFoundResult();

    private static ActionResult HandleInventoryNotFound(ExceptionContext context)
      => new NotFoundObjectResult(new
      {
        ItemId = context.Exception.Data["ItemId"],
        TrainerId = context.Exception.Data["TrainerId"]
      });

    private static ActionResult HandleMovesNotFound(ExceptionContext context)
      => new NotFoundObjectResult(new { Ids = context.Exception.Data["Ids"] });

    private static ActionResult HandleNatureNotFound(ExceptionContext context)
      => new NotFoundObjectResult(new { Field = context.Exception.Data["ParamName"] });

    private static ActionResult HandlePokedexEntryNotFound(ExceptionContext context)
      => new NotFoundResult();

    private static ActionResult HandlePokemonMoveNotFound(ExceptionContext context)
      => new NotFoundResult();

    private static ActionResult HandlePokemonNotFound(ExceptionContext context)
      => new NotFoundObjectResult(new { Ids = context.Exception.Data["Ids"] });
  }
}
