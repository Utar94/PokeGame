using Logitar.Portal.Contracts.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Abilities.Commands;
using PokeGame.Application.Abilities.Models;
using PokeGame.Application.Abilities.Queries;
using PokeGame.Models.Ability;

namespace PokeGame.Controllers;

[ApiController]
[Authorize]
[Route("abilities")]
public class AbilityController : ControllerBase
{
  private readonly IMediator _mediator;

  public AbilityController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<ActionResult<AbilityModel>> CreateAsync([FromBody] CreateOrReplaceAbilityPayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceAbilityCommand command = new(Id: null, payload, Version: null);
    CreateOrReplaceAbilityResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<AbilityModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    ReadAbilityQuery command = new(id, UniqueName: null);
    AbilityModel? ability = await _mediator.Send(command, cancellationToken);
    return ability == null ? NotFound() : Ok(ability);
  }

  [HttpGet("name:{uniqueName}")]
  public async Task<ActionResult<AbilityModel>> ReadAsync(string uniqueName, CancellationToken cancellationToken)
  {
    ReadAbilityQuery command = new(Id: null, uniqueName);
    AbilityModel? ability = await _mediator.Send(command, cancellationToken);
    return ability == null ? NotFound() : Ok(ability);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<AbilityModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceAbilityPayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceAbilityCommand command = new(id, payload, version);
    CreateOrReplaceAbilityResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<AbilityModel>>> SearchAsync([FromQuery] SearchAbilitiesParameters parameters, CancellationToken cancellationToken)
  {
    SearchAbilitiesPayload payload = parameters.ToPayload();
    SearchAbilitiesQuery query = new(payload);
    SearchResults<AbilityModel> abilities = await _mediator.Send(query, cancellationToken);
    return Ok(abilities);
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<AbilityModel>> UpdateAsync(Guid id, [FromBody] UpdateAbilityPayload payload, CancellationToken cancellationToken)
  {
    UpdateAbilityCommand command = new(id, payload);
    AbilityModel? ability = await _mediator.Send(command, cancellationToken);
    return ability == null ? NotFound() : Ok(ability);
  }

  private ActionResult ToActionResult(CreateOrReplaceAbilityResult result)
  {
    if (result.Ability == null)
    {
      return NotFound();
    }
    else if (result.Created)
    {
      Uri location = new($"{Request.Scheme}://{Request.Host}/abilities/{result.Ability.Id}", UriKind.Absolute);
      return Created(location, result.Ability);
    }

    return Ok(result.Ability);
  }
}
