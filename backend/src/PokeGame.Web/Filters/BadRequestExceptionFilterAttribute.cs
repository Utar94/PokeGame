using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokeGame.Application.Pokemon;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Trainers;

namespace PokeGame.Web.Filters
{
  internal class BadRequestExceptionFilterAttribute : ExceptionFilterAttribute
  {
    private static readonly Dictionary<Type, string> _codes = new()
    {
      [typeof(CannotCatchTrainerPokemonException)] = "CannotCatchTrainerPokemon",
      [typeof(CannotWoundFaintedPokemonException)] = "CannotWoundFaintedPokemon",
      [typeof(InsufficientMoneyException)] = "InsufficientMoney",
      [typeof(InsufficientQuantityException)] = "InsufficientQuantity",
      [typeof(InvalidAbilityException)] = "InvalidAbility",
      [typeof(ItemPriceRequiredException)] = "ItemPriceRequired",
      [typeof(NoAvailablePositionException)] = "NoAvailablePosition",
      [typeof(NoRemainingPowerPointException)] = "NoRemainingPowerPoint",
      [typeof(RemainingPowerPointsExceededException)] = "RemainingPowerPointsExceeded"
    };

    public override void OnException(ExceptionContext context)
    {
      if (_codes.TryGetValue(context.Exception.GetType(), out string? code))
      {
        context.ExceptionHandled = true;
        context.Result = new BadRequestObjectResult(new { code });
      }
    }
  }
}
