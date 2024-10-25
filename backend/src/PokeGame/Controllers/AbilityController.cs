using Logitar.Portal.Contracts.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application;
using PokeGame.Application.Abilities.Commands;
using PokeGame.Application.Abilities.Queries;
using PokeGame.Constants;
using PokeGame.Contracts.Abilities;
using PokeGame.Extensions;
using PokeGame.Models.Abilities;

namespace PokeGame.Controllers;

[ApiController]
[Authorize(Policy = Policies.Administrator)]
[Route("abilities")]
public class AbilityController : ControllerBase
{
  private readonly IRequestPipeline _pipeline;

  public AbilityController(IRequestPipeline pipeline)
  {
    _pipeline = pipeline;
  }

  [HttpPost]
  public async Task<ActionResult<AbilityModel>> CreateAsync([FromBody] CreateOrReplaceAbilityPayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceAbilityResult result = await _pipeline.ExecuteAsync(new CreateOrReplaceAbilityCommand(Id: null, payload, Version: null), cancellationToken);
    return GetActionResult(result);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<AbilityModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
  {
    AbilityModel? ability = await _pipeline.ExecuteAsync(new DeleteAbilityCommand(id), cancellationToken);
    return GetActionResult(ability);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<AbilityModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    AbilityModel? ability = await _pipeline.ExecuteAsync(new ReadAbilityQuery(id), cancellationToken);
    return GetActionResult(ability);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<AbilityModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceAbilityPayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceAbilityResult result = await _pipeline.ExecuteAsync(new CreateOrReplaceAbilityCommand(id, payload, version), cancellationToken);
    return GetActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<AbilityModel>>> SearchAsync([FromQuery] SearchAbilitiesParameters parameters, CancellationToken cancellationToken)
  {
    SearchResults<AbilityModel> abilities = await _pipeline.ExecuteAsync(new SearchAbilitiesQuery(parameters.ToPayload()), cancellationToken);
    return Ok(abilities);
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<AbilityModel>> UpdateAsync(Guid id, [FromBody] UpdateAbilityPayload payload, CancellationToken cancellationToken)
  {
    AbilityModel? ability = await _pipeline.ExecuteAsync(new UpdateAbilityCommand(id, payload), cancellationToken);
    return GetActionResult(ability);
  }

  private ActionResult<AbilityModel> GetActionResult(CreateOrReplaceAbilityResult result) => GetActionResult(result.Ability, result.Created);
  private ActionResult<AbilityModel> GetActionResult(AbilityModel? ability, bool created = false)
  {
    if (ability == null)
    {
      return NotFound();
    }
    if (created)
    {
      Uri location = HttpContext.BuildLocation("abilities/{id}", [new KeyValuePair<string, string>("id", ability.Id.ToString())]);
      return Created(location, ability);
    }

    return Ok(ability);
  }
}
