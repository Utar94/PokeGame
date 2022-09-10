using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Models;
using PokeGame.Application.Pokemon;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Pokemon.Payloads;
using PokeGame.Web.Models.Api.Pokemon;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("api/pokemon")]
  public class PokemonApiController : ControllerBase
  {
    private readonly IPokemonService _service;

    public PokemonApiController(IPokemonService service)
    {
      _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<PokemonModel>> CreateAsync([FromBody] CreatePokemonPayload payload, CancellationToken cancellationToken)
    {
      PokemonModel pokemon = await _service.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/api/pokemon/{pokemon.Id}", UriKind.Relative);

      return Created(uri, pokemon);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<PokemonModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      await _service.DeleteAsync(id, cancellationToken);

      return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<PokemonSummary>> GetAsync(PokemonGender? gender, string? search, Guid? speciesId, Guid? trainerId,
      PokemonSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      ListModel<PokemonModel> pokemon = await _service.GetAsync(gender, search, speciesId, trainerId,
        sort, desc,
        index, count,
        cancellationToken);

      return Ok(new ListModel<PokemonSummary>
      {
        Items = pokemon.Items.Select(x => new PokemonSummary(x)),
        Total = pokemon.Total
      });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PokemonModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      PokemonModel? pokemon = await _service.GetAsync(id, cancellationToken);
      if (pokemon == null)
      {
        return NotFound();
      }

      return Ok(pokemon);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PokemonModel>> UpdateAsync(Guid id, [FromBody] UpdatePokemonPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _service.UpdateAsync(id, payload, cancellationToken));
    }

    [HttpPatch("{id}/heal")]
    public async Task<ActionResult<PokemonModel>> HealAsync(Guid id, [FromBody] HealPokemonPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _service.HealAsync(id, payload.RestoreHitPoints, payload.RemoveCondition, cancellationToken));
    }
  }
}
