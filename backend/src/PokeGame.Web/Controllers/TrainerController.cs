using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Core.Trainers;
using PokeGame.Core.Trainers.Models;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("trainers")]
  public class TrainerController : Controller
  {
    private readonly ITrainerService _trainerService;

    public TrainerController(ITrainerService trainerService)
    {
      _trainerService = trainerService;
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
      TrainerModel? trainer = await _trainerService.GetAsync(id, cancellationToken);
      if (trainer == null)
      {
        return NotFound();
      }

      return View(trainer);
    }
  }
}
