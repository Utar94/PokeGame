using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Abilities;
using PokeGame.Application.Abilities.Models;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("abilities")]
  public class AbilityController : Controller
  {
    private readonly IAbilityService _abilityService;

    public AbilityController(IAbilityService abilityService)
    {
      _abilityService = abilityService;
    }

    [HttpGet("/create-ability")]
    public ActionResult CreateAbility()
    {
      return View(nameof(AbilityEdit));
    }

    [HttpGet]
    public ActionResult AbilityList()
    {
      return View();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> AbilityEdit(Guid id, CancellationToken cancellationToken = default)
    {
      AbilityModel? ability = await _abilityService.GetAsync(id, cancellationToken);
      if (ability == null)
      {
        return NotFound();
      }

      return View(ability);
    }
  }
}
