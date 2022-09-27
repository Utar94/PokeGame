using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokeGame.Application.Pokemon;
using PokeGame.Application.Species;

namespace PokeGame.Web.Filters
{
  internal class ConflictExceptionFilterAttribute : ExceptionFilterAttribute
  {
    private static readonly Dictionary<Type, string> _codes = new()
    {
      [typeof(PokemonPositionAlreadyUsedException)] = "PositionAlreadyUsed"
    };

    public override void OnException(ExceptionContext context)
    {
      if (_codes.TryGetValue(context.Exception.GetType(), out string? code))
      {
        context.ExceptionHandled = true;
        context.Result = new ConflictObjectResult(new { code });
      }
      else if (context.Exception is SpeciesNumberAlreadyUsedException)
      {
        context.ExceptionHandled = true;
        context.Result = new ConflictObjectResult(new { Field = context.Exception.Data["ParamName"] });
      }
      else if (context.Exception is RegionalNumbersAlreadyUsedException)
      {
        context.ExceptionHandled = true;
        context.Result = new ConflictObjectResult(new
        {
          Field = context.Exception.Data["ParamName"],
          RegionalNumbers = context.Exception.Data["RegionalNumbers"]
        });
      }
    }
  }
}
