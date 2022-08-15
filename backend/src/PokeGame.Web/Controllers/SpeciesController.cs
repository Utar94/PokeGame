using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Core.Species;
using PokeGame.Core.Species.Models;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("species")]
  public class SpeciesController : Controller
  {
    private readonly ISpeciesService _speciesService;

    public SpeciesController(ISpeciesService speciesService)
    {
      _speciesService = speciesService;
    }

    [HttpGet("/create-species")]
    public ActionResult CreateSpecies()
    {
      return View(nameof(SpeciesEdit));
    }

    [HttpGet]
    public ActionResult SpeciesList()
    {
      return View();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> SpeciesEdit(Guid id, CancellationToken cancellationToken = default)
    {
      SpeciesModel? species = await _speciesService.GetAsync(id, cancellationToken);
      if (species == null)
      {
        return NotFound();
      }

      return View(species);
    }
  }
}
