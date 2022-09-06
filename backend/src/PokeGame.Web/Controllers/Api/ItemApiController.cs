using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Items;
using PokeGame.Application.Items.Models;
using PokeGame.Application.Models;
using PokeGame.Domain.Items;
using PokeGame.Domain.Items.Payloads;
using PokeGame.Web.Models.Api.Items;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("api/items")]
  public class ItemApiController : ControllerBase
  {
    private readonly IItemService _service;

    public ItemApiController(IItemService service)
    {
      _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ItemModel>> CreateAsync([FromBody] CreateItemPayload payload, CancellationToken cancellationToken)
    {
      ItemModel item = await _service.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/api/items/{item.Id}", UriKind.Relative);

      return Created(uri, item);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ItemModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      await _service.DeleteAsync(id, cancellationToken);

      return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<ItemSummary>> GetAsync(ItemCategory? category, string? search,
      ItemSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      ListModel<ItemModel> items = await _service.GetAsync(category, search,
        sort, desc,
        index, count,
        cancellationToken);

      return Ok(new ListModel<ItemSummary>
      {
        Items = items.Items.Select(x => new ItemSummary(x)),
        Total = items.Total
      });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      ItemModel? item = await _service.GetAsync(id, cancellationToken);
      if (item == null)
      {
        return NotFound();
      }

      return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ItemModel>> UpdateAsync(Guid id, [FromBody] UpdateItemPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _service.UpdateAsync(id, payload, cancellationToken));
    }
  }
}
