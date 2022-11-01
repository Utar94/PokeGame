using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Regions;
using PokeGame.Application.Regions.Models;
using PokeGame.Application.Regions.Mutations;
using PokeGame.Application.Regions.Queries;
using PokeGame.Domain.Regions.Payloads;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("api/regions")]
  public class RegionApiController : ControllerBase
  {
    private readonly IMediator _mediator;

    public RegionApiController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<RegionModel>> CreateAsync([FromBody] CreateRegionPayload payload, CancellationToken cancellationToken)
    {
      RegionModel region = await _mediator.Send(new CreateRegionMutation(payload), cancellationToken);
      var uri = new Uri($"/api/regions/{region.Id}", UriKind.Relative);

      return Created(uri, region);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<RegionModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      await _mediator.Send(new DeleteRegionMutation(id), cancellationToken);

      return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<RegionModel>> GetAsync(string? search,
      RegionSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new GetRegionsQuery
      {
        Search = search,
        Sort = sort,
        Desc = desc,
        Index = index,
        Count = count
      }, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RegionModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      RegionModel? region = await _mediator.Send(new GetRegionQuery(id), cancellationToken);
      if (region == null)
      {
        return NotFound();
      }

      return Ok(region);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RegionModel>> UpdateAsync(Guid id, [FromBody] UpdateRegionPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new UpdateRegionMutation(id, payload), cancellationToken));
    }
  }
}
