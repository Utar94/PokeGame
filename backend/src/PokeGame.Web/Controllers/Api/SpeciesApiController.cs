using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Models;
using PokeGame.Application.Species;
using PokeGame.Application.Species.Models;
using PokeGame.Domain;
using PokeGame.Domain.Species.Payloads;
using PokeGame.Web.Models.Api.Species;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("api/species")]
  public class SpeciesApiController : ControllerBase
  {
    private readonly ISpeciesService _service;

    public SpeciesApiController(ISpeciesService service)
    {
      _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<SpeciesModel>> CreateAsync([FromBody] CreateSpeciesPayload payload, CancellationToken cancellationToken)
    {
      SpeciesModel species = await _service.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/api/species/{species.Id}", UriKind.Relative);

      return Created(uri, species);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<SpeciesModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      await _service.DeleteAsync(id, cancellationToken);

      return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<SpeciesSummary>> GetAsync(string? search, PokemonType? type,
      SpeciesSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      ListModel<SpeciesModel> species = await _service.GetAsync(search, type,
        sort, desc,
        index, count,
        cancellationToken);

      return Ok(new ListModel<SpeciesSummary>
      {
        Items = species.Items.Select(x => new SpeciesSummary(x)),
        Total = species.Total
      });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SpeciesModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      SpeciesModel? species = await _service.GetAsync(id, cancellationToken);
      if (species == null)
      {
        return NotFound();
      }

      return Ok(species);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SpeciesModel>> UpdateAsync(Guid id, [FromBody] UpdateSpeciesPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _service.UpdateAsync(id, payload, cancellationToken));
    }
  }
}
