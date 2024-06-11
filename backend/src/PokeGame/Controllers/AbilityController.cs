using Logitar.Portal.Contracts.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application;
using PokeGame.Application.Abilities.Commands;
using PokeGame.Application.Abilities.Queries;
using PokeGame.Contracts.Abilities;
using PokeGame.Extensions;
using PokeGame.Models.Abilities;

namespace PokeGame.Controllers;

[ApiController]
[Authorize] // TODO(fpion): RBAC
[Route("abilities")]
public class AbilityController : ControllerBase
{
  private readonly IRequestPipeline _pipeline;

  public AbilityController(IRequestPipeline pipeline)
  {
    _pipeline = pipeline;
  }

  [HttpPost]
  public async Task<ActionResult<Ability>> CreateAsync([FromBody] CreateAbilityPayload payload, CancellationToken cancellationToken)
  {
    Ability ability = await _pipeline.ExecuteAsync(new CreateAbilityCommand(payload), cancellationToken);
    Uri location = HttpContext.BuildLocation("abilities", new Dictionary<string, string>
    {
      ["id"] = ability.Id.ToString()
    });
    return Created(location, ability);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Ability>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    Ability? ability = await _pipeline.ExecuteAsync(new ReadAbilityQuery(id, UniqueName: null), cancellationToken);
    return ability == null ? NotFound() : Ok(ability);
  }

  [HttpGet("unique-name:{uniqueName}")]
  public async Task<ActionResult<Ability>> ReadAsync(string uniqueName, CancellationToken cancellationToken)
  {
    Ability? ability = await _pipeline.ExecuteAsync(new ReadAbilityQuery(Id: null, uniqueName), cancellationToken);
    return ability == null ? NotFound() : Ok(ability);
  }

  //[HttpPut("{id}")]
  //public async Task<ActionResult<Ability>> ReplaceAsync(Guid id, [FromBody] ReplaceAbilityPayload payload, long? version, CancellationToken cancellationToken = default)
  //{
  //  Ability? ability = await _pipeline.ExecuteAsync(new ReplaceAbilityCommand(id, payload, version), cancellationToken);
  //  return ability == null ? NotFound() : Ok(ability);
  //} // TODO(fpion): Replace

  [HttpGet]
  public async Task<ActionResult<SearchResults<Ability>>> SearchAsync([FromQuery] SearchAbilitiesParameters parameters, CancellationToken cancellationToken)
  {
    SearchResults<Ability> abilities = await _pipeline.ExecuteAsync(new SearchAbilitiesQuery(parameters.ToPayload()), cancellationToken);
    return Ok(abilities);
  }
}
