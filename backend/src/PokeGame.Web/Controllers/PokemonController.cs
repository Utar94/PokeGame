using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Pokemon;
using PokeGame.Application.Pokemon.Models;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("pokemon")]
  public class PokemonController : Controller
  {
    private readonly IPokemonService _pokemonService;

    public PokemonController(IPokemonService pokemonService)
    {
      _pokemonService = pokemonService;
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
      PokemonModel? pokemon = await _pokemonService.GetAsync(id, cancellationToken);
      if (pokemon == null)
      {
        return NotFound();
      }

      return View(pokemon);
    }
  }
}
