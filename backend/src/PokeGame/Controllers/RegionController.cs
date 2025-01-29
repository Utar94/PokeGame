using Logitar.Portal.Contracts.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Regions.Commands;
using PokeGame.Application.Regions.Models;
using PokeGame.Application.Regions.Queries;
using PokeGame.Models.Region;

namespace PokeGame.Controllers;

[ApiController]
[Authorize]
[Route("regions")]
public class RegionController : ControllerBase
{
  private readonly IMediator _mediator;

  public RegionController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<ActionResult<RegionModel>> CreateAsync([FromBody] CreateOrReplaceRegionPayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceRegionCommand command = new(Id: null, payload, Version: null);
    CreateOrReplaceRegionResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<RegionModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    ReadRegionQuery command = new(id, UniqueName: null);
    RegionModel? region = await _mediator.Send(command, cancellationToken);
    return region == null ? NotFound() : Ok(region);
  }

  [HttpGet("name:{uniqueName}")]
  public async Task<ActionResult<RegionModel>> ReadAsync(string uniqueName, CancellationToken cancellationToken)
  {
    ReadRegionQuery command = new(Id: null, uniqueName);
    RegionModel? region = await _mediator.Send(command, cancellationToken);
    return region == null ? NotFound() : Ok(region);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<RegionModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceRegionPayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceRegionCommand command = new(id, payload, version);
    CreateOrReplaceRegionResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<RegionModel>>> SearchAsync([FromQuery] SearchRegionsParameters parameters, CancellationToken cancellationToken)
  {
    SearchRegionsPayload payload = parameters.ToPayload();
    SearchRegionsQuery query = new(payload);
    SearchResults<RegionModel> regions = await _mediator.Send(query, cancellationToken);
    return Ok(regions);
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<RegionModel>> UpdateAsync(Guid id, [FromBody] UpdateRegionPayload payload, CancellationToken cancellationToken)
  {
    UpdateRegionCommand command = new(id, payload);
    RegionModel? region = await _mediator.Send(command, cancellationToken);
    return region == null ? NotFound() : Ok(region);
  }

  private ActionResult ToActionResult(CreateOrReplaceRegionResult result)
  {
    if (result.Region == null)
    {
      return NotFound();
    }
    else if (result.Created)
    {
      Uri location = new($"{Request.Scheme}://{Request.Host}/regions/{result.Region.Id}", UriKind.Absolute);
      return Created(location, result.Region);
    }

    return Ok(result.Region);
  }
}
