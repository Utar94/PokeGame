using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Models;
using PokeGame.Application.Pokedex;
using PokeGame.Application.Pokedex.Models;
using PokeGame.Application.Pokedex.Mutations;
using PokeGame.Application.Pokedex.Payloads;
using PokeGame.Application.Pokedex.Queries;
using PokeGame.Domain;
using PokeGame.Web.Models.Api.Pokedex;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("api/trainers/{trainerId}/pokedex")]
  public class PokedexApiController : ControllerBase
  {
    private readonly IMediator _mediator;

    public PokedexApiController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpDelete("{speciesId}")]
    public async Task<ActionResult<PokedexModel>> DeleteAsync(Guid trainerId, Guid speciesId, CancellationToken cancellationToken)
    {
      await _mediator.Send(new DeletePokedexEntryMutation(trainerId, speciesId), cancellationToken);

      return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<ListModel<PokedexModel>>> GetAsync(Guid trainerId, bool? hasCaught, Region? region, string? search, PokemonType? type,
      PokedexSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new GetPokedexEntriesQuery
      {
        TrainerId = trainerId,
        HasCaught = hasCaught,
        Region = region,
        Search = search,
        Type = type,
        Sort = sort,
        Desc = desc,
        Index = index,
        Count = count
      }, cancellationToken));
    }

    [HttpGet("{speciesId}")]
    public async Task<ActionResult<PokedexModel>> GetAsync(Guid trainerId, Guid speciesId, CancellationToken cancellationToken)
    {
      PokedexModel? model = await _mediator.Send(new GetPokedexEntryQuery(trainerId, speciesId), cancellationToken);
      if (model == null)
      {
        return NotFound();
      }

      return Ok(model);
    }

    [HttpPut("{speciesId}")]
    public async Task<ActionResult<PokedexModel>> SaveAsync(Guid trainerId, Guid speciesId, [FromBody] PokedexPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new SavePokedexEntryMutation(trainerId, speciesId, payload.HasCaught), cancellationToken));
    }

    [HttpPatch("/api/pokedex/seen")]
    public async Task<ActionResult> SeenAsync([FromBody] SeenSpeciesPayload payload, CancellationToken cancellationToken)
    {
      await _mediator.Send(new SeenSpeciesMutation(payload), cancellationToken);

      return NoContent();
    }
  }
}
