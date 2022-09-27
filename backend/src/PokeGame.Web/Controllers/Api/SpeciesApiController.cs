using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Species;
using PokeGame.Application.Species.Models;
using PokeGame.Application.Species.Mutations;
using PokeGame.Application.Species.Queries;
using PokeGame.Domain;
using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("api/species")]
  public class SpeciesApiController : ControllerBase
  {
    private readonly IMediator _mediator;

    public SpeciesApiController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<SpeciesModel>> CreateAsync([FromBody] CreateSpeciesPayload payload, CancellationToken cancellationToken)
    {
      SpeciesModel species = await _mediator.Send(new CreateSpeciesMutation(payload), cancellationToken);
      var uri = new Uri($"/api/species/{species.Id}", UriKind.Relative);

      return Created(uri, species);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<SpeciesModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      await _mediator.Send(new DeleteSpeciesMutation(id), cancellationToken);

      return NoContent();
    }

    [HttpDelete("{id}/evolutions/{speciesId}")]
    public async Task<ActionResult> RemoveEvolutionAsync(Guid id, Guid speciesId, CancellationToken cancellationToken)
    {
      await _mediator.Send(new RemoveSpeciesEvolutionMutation(id, speciesId), cancellationToken);

      return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<SpeciesModel>> GetAsync(Region? region, string? search, PokemonType? type,
      SpeciesSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new GetSpeciesListQuery
      {
        Region = region,
        Search = search,
        Type = type,
        Sort = sort,
        Desc = desc,
        Index = index,
        Count = count
      }, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SpeciesModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      SpeciesModel? species = await _mediator.Send(new GetSpeciesQuery(id), cancellationToken);
      if (species == null)
      {
        return NotFound();
      }

      return Ok(species);
    }

    [HttpGet("{id}/evolutions")]
    public async Task<ActionResult<IEnumerable<EvolutionModel>>> GetEvolutionsAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new GetSpeciesEvolutionsQuery(id), cancellationToken));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SpeciesModel>> UpdateAsync(Guid id, [FromBody] UpdateSpeciesPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new UpdateSpeciesMutation(id, payload), cancellationToken));
    }

    [HttpPut("{id}/evolutions/{speciesId}")]
    public async Task<ActionResult<EvolutionModel>> SaveEvolutionAsync(Guid id, Guid speciesId, [FromBody] SaveEvolutionPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new SaveSpeciesEvolutionMutation(id, speciesId, payload), cancellationToken));
    }
  }
}
