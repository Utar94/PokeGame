using Logitar.Portal.Contracts.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Speciez.Commands;
using PokeGame.Application.Speciez.Models;
using PokeGame.Application.Speciez.Queries;
using PokeGame.Models.Species;

namespace PokeGame.Controllers;

[ApiController]
[Authorize]
[Route("species")]
public class SpeciesController : ControllerBase
{
  private readonly IMediator _mediator;

  public SpeciesController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<ActionResult<SpeciesModel>> CreateAsync([FromBody] CreateOrReplaceSpeciesPayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceSpeciesCommand command = new(Id: null, payload, Version: null);
    CreateOrReplaceSpeciesResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<SpeciesModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    ReadSpeciesQuery command = new(id, UniqueName: null);
    SpeciesModel? species = await _mediator.Send(command, cancellationToken);
    return species == null ? NotFound() : Ok(species);
  }

  [HttpGet("name:{uniqueName}")]
  public async Task<ActionResult<SpeciesModel>> ReadAsync(string uniqueName, CancellationToken cancellationToken)
  {
    ReadSpeciesQuery command = new(Id: null, uniqueName);
    SpeciesModel? species = await _mediator.Send(command, cancellationToken);
    return species == null ? NotFound() : Ok(species);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<SpeciesModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceSpeciesPayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceSpeciesCommand command = new(id, payload, version);
    CreateOrReplaceSpeciesResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<SpeciesModel>>> SearchAsync([FromQuery] SearchSpeciesParameters parameters, CancellationToken cancellationToken)
  {
    SearchSpeciesPayload payload = parameters.ToPayload();
    SearchSpeciesQuery query = new(payload);
    SearchResults<SpeciesModel> speciess = await _mediator.Send(query, cancellationToken);
    return Ok(speciess);
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<SpeciesModel>> UpdateAsync(Guid id, [FromBody] UpdateSpeciesPayload payload, CancellationToken cancellationToken)
  {
    UpdateSpeciesCommand command = new(id, payload);
    SpeciesModel? species = await _mediator.Send(command, cancellationToken);
    return species == null ? NotFound() : Ok(species);
  }

  private ActionResult ToActionResult(CreateOrReplaceSpeciesResult result)
  {
    if (result.Species == null)
    {
      return NotFound();
    }
    else if (result.Created)
    {
      Uri location = new($"{Request.Scheme}://{Request.Host}/species/{result.Species.Id}", UriKind.Absolute);
      return Created(location, result.Species);
    }

    return Ok(result.Species);
  }
}
