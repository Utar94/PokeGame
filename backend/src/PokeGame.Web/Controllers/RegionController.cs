using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Regions.Models;
using PokeGame.Application.Regions.Queries;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("regions")]
  public class RegionController : Controller
  {
    private readonly IMediator _mediator;

    public RegionController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpGet("/create-region")]
    public ActionResult CreateRegion()
    {
      return View(nameof(RegionEdit));
    }

    [HttpGet]
    public ActionResult RegionList()
    {
      return View();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> RegionEdit(Guid id, CancellationToken cancellationToken = default)
    {
      RegionModel? region = await _mediator.Send(new GetRegionQuery(id), cancellationToken);
      if (region == null)
      {
        return NotFound();
      }

      return View(region);
    }
  }
}
