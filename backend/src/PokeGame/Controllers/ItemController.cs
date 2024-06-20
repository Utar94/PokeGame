using Logitar.Portal.Contracts.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application;
using PokeGame.Application.Items.Commands;
using PokeGame.Application.Items.Queries;
using PokeGame.Constants;
using PokeGame.Contracts.Items;
using PokeGame.Extensions;
using PokeGame.Models.Items;

namespace PokeGame.Controllers;

[ApiController]
[Authorize(Policy = Policies.Gamemaster)]
[Route("items")]
public class ItemController : ControllerBase
{
  private readonly IRequestPipeline _pipeline;

  public ItemController(IRequestPipeline pipeline)
  {
    _pipeline = pipeline;
  }

  [HttpPost]
  public async Task<ActionResult<Item>> CreateAsync([FromBody] CreateItemPayload payload, CancellationToken cancellationToken)
  {
    Item item = await _pipeline.ExecuteAsync(new CreateItemCommand(payload), cancellationToken);
    Uri location = HttpContext.BuildLocation("items", new Dictionary<string, string>
    {
      ["id"] = item.Id.ToString()
    });
    return Created(location, item);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<Item>> DeleteAsync(Guid id, CancellationToken cancellationToken)
  {
    Item? item = await _pipeline.ExecuteAsync(new DeleteItemCommand(id), cancellationToken);
    return item == null ? NotFound() : Ok(item);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Item>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    Item? item = await _pipeline.ExecuteAsync(new ReadItemQuery(id, UniqueName: null), cancellationToken);
    return item == null ? NotFound() : Ok(item);
  }

  [HttpGet("unique-name:{uniqueName}")]
  public async Task<ActionResult<Item>> ReadAsync(string uniqueName, CancellationToken cancellationToken)
  {
    Item? item = await _pipeline.ExecuteAsync(new ReadItemQuery(Id: null, uniqueName), cancellationToken);
    return item == null ? NotFound() : Ok(item);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Item>> ReplaceAsync(Guid id, [FromBody] ReplaceItemPayload payload, long? version, CancellationToken cancellationToken = default)
  {
    Item? item = await _pipeline.ExecuteAsync(new ReplaceItemCommand(id, payload, version), cancellationToken);
    return item == null ? NotFound() : Ok(item);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<Item>>> SearchAsync([FromQuery] SearchItemsParameters parameters, CancellationToken cancellationToken)
  {
    SearchResults<Item> items = await _pipeline.ExecuteAsync(new SearchItemsQuery(parameters.ToPayload()), cancellationToken);
    return Ok(items);
  }
}
