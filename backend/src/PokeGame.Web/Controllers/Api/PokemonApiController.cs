using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Pokemon;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Application.Pokemon.Mutations;
using PokeGame.Application.Pokemon.Queries;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("api/pokemon")]
  public class PokemonApiController : ControllerBase
  {
    private readonly IMediator _mediator;

    public PokemonApiController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<PokemonModel>> CreateAsync([FromBody] CreatePokemonPayload payload, CancellationToken cancellationToken)
    {
      PokemonModel pokemon = await _mediator.Send(new CreatePokemonMutation(payload), cancellationToken);
      var uri = new Uri($"/api/pokemon/{pokemon.Id}", UriKind.Relative);

      return Created(uri, pokemon);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<PokemonModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      await _mediator.Send(new DeletePokemonMutation(id), cancellationToken);

      return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<PokemonModel>> GetAsync(PokemonGender? gender, byte? inBox, bool? inParty, bool? isWild, string? search, Guid? speciesId, Guid? trainerId,
      PokemonSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new GetPokemonListQuery
      {
        Gender = gender,
        InBox = inBox,
        InParty = inParty,
        IsWild = isWild,
        Search = search,
        SpeciesId = speciesId,
        TrainerId = trainerId,
        Sort = sort,
        Desc = desc,
        Index = index,
        Count = count
      }, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PokemonModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      PokemonModel? pokemon = await _mediator.Send(new GetPokemonQuery(id), cancellationToken);
      if (pokemon == null)
      {
        return NotFound();
      }

      return Ok(pokemon);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PokemonModel>> UpdateAsync(Guid id, [FromBody] UpdatePokemonPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new UpdatePokemonMutation(id, payload), cancellationToken));
    }

    [HttpPatch("{id}/catch")]
    public async Task<ActionResult<PokemonModel>> CatchAsync(Guid id, [FromBody] CatchPokemonPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new CatchPokemonMutation(id, payload), cancellationToken));
    }

    [HttpPatch("{id}/condition")]
    public async Task<ActionResult<PokemonModel>> UpdateConditionAsync(Guid id, [FromBody] UpdatePokemonConditionPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new UpdatePokemonConditionMutation(id, payload), cancellationToken));
    }

    [HttpPatch("{id}/evolve")]
    public async Task<ActionResult<PokemonModel>> EvolveAsync(Guid id, [FromBody] EvolvePokemonPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new EvolvePokemonMutation(id, payload), cancellationToken));
    }

    [HttpPatch("{id}/gain")]
    public async Task<ActionResult<PokemonModel>> GainAsync(Guid id, [FromBody] ExperienceGainPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new PokemonGainMutation(id, payload), cancellationToken));
    }

    [HttpPatch("{id}/heal")]
    public async Task<ActionResult<PokemonModel>> HealAsync(Guid id, [FromBody] HealPokemonPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new HealPokemonMutation(id, payload), cancellationToken));
    }

    [HttpPatch("{id}/hold-item/remove")]
    public async Task<ActionResult<PokemonModel>> RemoveHoldItemAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new HoldItemPokemonMutation(id), cancellationToken));
    }

    [HttpPatch("{id}/hold-item/{itemId}")]
    public async Task<ActionResult<PokemonModel>> HoldItemAsync(Guid id, Guid itemId, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new HoldItemPokemonMutation(id, itemId), cancellationToken));
    }

    [HttpPatch("{id}/swap/{otherId}")]
    public async Task<ActionResult<PokemonModel>> SwapAsync(Guid id, Guid otherId, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new SwapPokemonMutation(id, otherId), cancellationToken));
    }

    [HttpPatch("{id}/use-move/{moveId}")]
    public async Task<ActionResult<PokemonModel>> UseMoveAsync(Guid id, Guid moveId, [FromBody] UsePokemonMovePayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new UsePokemonMoveMutation(id, moveId, payload), cancellationToken));
    }
  }
}
