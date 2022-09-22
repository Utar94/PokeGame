using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Species.Models;
using PokeGame.Application.Species.Queries;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("species")]
  public class SpeciesController : Controller
  {
    private readonly IMediator _mediator;

    public SpeciesController(IMediator mediator)
    {
      _mediator = mediator;
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
      SpeciesModel? species = await _mediator.Send(new GetSpeciesQuery(id), cancellationToken);
      if (species == null)
      {
        return NotFound();
      }

      return View(species);
    }
  }
}
