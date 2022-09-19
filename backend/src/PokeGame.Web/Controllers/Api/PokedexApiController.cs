using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Models;
using PokeGame.Application.Pokedex;
using PokeGame.Application.Pokedex.Models;
using PokeGame.Domain;
using PokeGame.Web.Models.Api.Pokedex;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("api/trainers/{trainerId}/pokedex")]
  public class PokedexApiController : ControllerBase
  {
    private readonly IPokedexService _service;

    public PokedexApiController(IPokedexService service)
    {
      _service = service;
    }

    [HttpDelete("{speciesId}")]
    public async Task<ActionResult<PokedexModel>> DeleteAsync(Guid trainerId, Guid speciesId, CancellationToken cancellationToken)
    {
      await _service.DeleteAsync(trainerId, speciesId, cancellationToken);

      return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<ListModel<PokedexModel>>> GetAsync(Guid trainerId, bool? hasCaught, string? search, PokemonType? type,
      PokedexSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _service.GetAsync(trainerId, hasCaught, search, type,
        sort, desc,
        index, count,
        cancellationToken));
    }

    [HttpGet("{speciesId}")]
    public async Task<ActionResult<PokedexModel>> GetAsync(Guid trainerId, Guid speciesId, CancellationToken cancellationToken)
    {
      PokedexModel? model = await _service.GetAsync(trainerId, speciesId, cancellationToken);
      if (model == null)
      {
        return NotFound();
      }

      return Ok(model);
    }

    [HttpPut("{speciesId}")]
    public async Task<ActionResult<PokedexModel>> SaveAsync(Guid trainerId, Guid speciesId, [FromBody] PokedexPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _service.SaveAsync(trainerId, speciesId, payload.HasCaught, cancellationToken));
    }
  }
}
