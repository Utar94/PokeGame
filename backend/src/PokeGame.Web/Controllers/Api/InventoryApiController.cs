using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Inventories;
using PokeGame.Application.Inventories.Models;
using PokeGame.Application.Models;
using PokeGame.Domain.Items;
using PokeGame.Web.Models.Api.Inventory;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("api/trainers/{trainerId}/inventory")]
  public class InventoryApiController : ControllerBase
  {
    private readonly IInventoryService _service;

    public InventoryApiController(IInventoryService service)
    {
      _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ListModel<InventoryModel>>> GetAsync(Guid trainerId, ItemCategory? category, string? search,
      InventorySort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _service.GetAsync(trainerId, category, search,
        sort, desc,
        index, count,
        cancellationToken));
    }

    [HttpGet("{itemId}")]
    public async Task<ActionResult<InventoryModel>> GetAsync(Guid trainerId, Guid itemId, CancellationToken cancellationToken)
    {
      return Ok(await _service.GetAsync(trainerId, itemId, cancellationToken));
    }

    [HttpPatch("{itemId}/add")]
    public async Task<ActionResult<InventoryModel>> AddAsync(Guid trainerId, Guid itemId, [FromBody] QuantityPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _service.AddAsync(trainerId, itemId, payload.Quantity, buy: false, cancellationToken));
    }

    [HttpPatch("{itemId}/buy")]
    public async Task<ActionResult<InventoryModel>> BuyAsync(Guid trainerId, Guid itemId, [FromBody] QuantityPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _service.AddAsync(trainerId, itemId, payload.Quantity, buy: true, cancellationToken));
    }

    [HttpPatch("{itemId}/remove")]
    public async Task<ActionResult<InventoryModel>> RemoveAsync(Guid trainerId, Guid itemId, [FromBody] QuantityPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _service.RemoveAsync(trainerId, itemId, payload.Quantity, sell: false, cancellationToken));
    }

    [HttpPatch("{itemId}/sell")]
    public async Task<ActionResult<InventoryModel>> SellAsync(Guid trainerId, Guid itemId, [FromBody] QuantityPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _service.RemoveAsync(trainerId, itemId, payload.Quantity, sell: true, cancellationToken));
    }
  }
}
