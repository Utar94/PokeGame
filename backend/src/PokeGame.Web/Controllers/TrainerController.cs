using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Trainers.Models;
using PokeGame.Application.Trainers.Queries;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("trainers")]
  public class TrainerController : Controller
  {
    private readonly IMediator _mediator;

    public TrainerController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpGet("/create-trainer")]
    public ActionResult CreateTrainer()
    {
      return View(nameof(TrainerEdit));
    }

    [HttpGet]
    public ActionResult TrainerList()
    {
      return View();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> TrainerEdit(Guid id, CancellationToken cancellationToken = default)
    {
      TrainerModel? trainer = await _mediator.Send(new GetTrainerQuery(id), cancellationToken);
      if (trainer == null)
      {
        return NotFound();
      }

      return View(trainer);
    }
  }
}
