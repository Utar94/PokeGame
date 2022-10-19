using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Moves;
using PokeGame.Application.Moves.Models;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("moves")]
  public class MoveController : Controller
  {
    private readonly IMoveService _moveService;

    public MoveController(IMoveService moveService)
    {
      _moveService = moveService;
    }

    [HttpGet("/create-move")]
    public ActionResult CreateMove()
    {
      return View(nameof(MoveEdit));
    }

    [HttpGet]
    public ActionResult MoveList()
    {
      return View();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> MoveEdit(Guid id, CancellationToken cancellationToken = default)
    {
      MoveModel? move = await _moveService.GetAsync(id, cancellationToken);
      if (move == null)
      {
        return NotFound();
      }

      return View(move);
    }
  }
}
