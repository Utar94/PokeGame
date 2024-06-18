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
[Authorize(Policy = Policies.Gamemaster)]
[Route("regions")]
public class RegionController : ControllerBase
{
  private readonly IRequestPipeline _pipeline;

  public RegionController(IRequestPipeline pipeline)
  {
    _pipeline = pipeline;
  }

  [HttpPost]
  public async Task<ActionResult<Region>> CreateAsync([FromBody] CreateRegionPayload payload, CancellationToken cancellationToken)
  {
    Region region = await _pipeline.ExecuteAsync(new CreateRegionCommand(payload), cancellationToken);
    Uri location = HttpContext.BuildLocation("regions", new Dictionary<string, string>
    {
      ["id"] = region.Id.ToString()
    });
    return Created(location, region);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<Region>> DeleteAsync(Guid id, CancellationToken cancellationToken)
  {
    Region? region = await _pipeline.ExecuteAsync(new ReadRegionQuery(id, UniqueName: null), cancellationToken);
    return region == null ? NotFound() : Ok(region);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Region>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    Region? region = await _pipeline.ExecuteAsync(new ReadRegionQuery(id, UniqueName: null), cancellationToken);
    return region == null ? NotFound() : Ok(region);
  }

  [HttpGet("unique-name:{uniqueName}")]
  public async Task<ActionResult<Region>> ReadAsync(string uniqueName, CancellationToken cancellationToken)
  {
    Region? region = await _pipeline.ExecuteAsync(new ReadRegionQuery(Id: null, uniqueName), cancellationToken);
    return region == null ? NotFound() : Ok(region);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Region>> ReplaceAsync(Guid id, [FromBody] ReplaceRegionPayload payload, long? version, CancellationToken cancellationToken = default)
  {
    Region? region = await _pipeline.ExecuteAsync(new ReplaceRegionCommand(id, payload, version), cancellationToken);
    return region == null ? NotFound() : Ok(region);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<Region>>> SearchAsync([FromQuery] SearchRegionsParameters parameters, CancellationToken cancellationToken)
  {
    SearchResults<Region> regions = await _pipeline.ExecuteAsync(new SearchRegionsQuery(parameters.ToPayload()), cancellationToken);
    return Ok(regions);
  }
}
