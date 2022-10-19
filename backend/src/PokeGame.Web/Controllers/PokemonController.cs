using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Application.Pokemon.Queries;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("pokemon")]
  public class PokemonController : Controller
  {
    private readonly IMediator _mediator;

    public PokemonController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpGet("/create-pokemon")]
    public ActionResult CreatePokemon()
    {
      return View(nameof(PokemonEdit));
    }

    [HttpGet]
    public ActionResult PokemonList()
    {
      return View();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> PokemonEdit(Guid id, CancellationToken cancellationToken = default)
    {
      PokemonModel? pokemon = await _mediator.Send(new GetPokemonQuery(id), cancellationToken);
      if (pokemon == null)
      {
        return NotFound();
      }

      return View(pokemon);
    }
  }
}
