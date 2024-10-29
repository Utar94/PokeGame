using Logitar.Portal.Contracts.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application;
using PokeGame.Application.Regions.Commands;
using PokeGame.Application.Regions.Queries;
using PokeGame.Constants;
using PokeGame.Contracts.Regions;
using PokeGame.Extensions;
using PokeGame.Models.Regions;

namespace PokeGame.Controllers;

[ApiController]
[Authorize(Policy = Policies.Administrator)]
[Route("regions")]
public class RegionController : ControllerBase
{
  private readonly IRequestPipeline _pipeline;

  public RegionController(IRequestPipeline pipeline)
  {
    _pipeline = pipeline;
  }

  [HttpPost]
  public async Task<ActionResult<RegionModel>> CreateAsync([FromBody] CreateOrReplaceRegionPayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceRegionResult result = await _pipeline.ExecuteAsync(new CreateOrReplaceRegionCommand(Id: null, payload, Version: null), cancellationToken);
    return GetActionResult(result);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<RegionModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
  {
    RegionModel? region = await _pipeline.ExecuteAsync(new DeleteRegionCommand(id), cancellationToken);
    return GetActionResult(region);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<RegionModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    RegionModel? region = await _pipeline.ExecuteAsync(new ReadRegionQuery(id), cancellationToken);
    return GetActionResult(region);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<RegionModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceRegionPayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceRegionResult result = await _pipeline.ExecuteAsync(new CreateOrReplaceRegionCommand(id, payload, version), cancellationToken);
    return GetActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<RegionModel>>> SearchAsync([FromQuery] SearchRegionsParameters parameters, CancellationToken cancellationToken)
  {
    SearchResults<RegionModel> regions = await _pipeline.ExecuteAsync(new SearchRegionsQuery(parameters.ToPayload()), cancellationToken);
    return Ok(regions);
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<RegionModel>> UpdateAsync(Guid id, [FromBody] UpdateRegionPayload payload, CancellationToken cancellationToken)
  {
    RegionModel? region = await _pipeline.ExecuteAsync(new UpdateRegionCommand(id, payload), cancellationToken);
    return GetActionResult(region);
  }

  private ActionResult<RegionModel> GetActionResult(CreateOrReplaceRegionResult result) => GetActionResult(result.Region, result.Created);
  private ActionResult<RegionModel> GetActionResult(RegionModel? region, bool created = false)
  {
    if (region == null)
    {
      return NotFound();
    }
    if (created)
    {
      Uri location = HttpContext.BuildLocation("regions/{id}", [new KeyValuePair<string, string>("id", region.Id.ToString())]);
      return Created(location, region);
    }

    return Ok(region);
  }
}
