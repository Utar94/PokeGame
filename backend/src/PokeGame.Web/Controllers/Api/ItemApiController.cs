using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Core.Items;
using PokeGame.Core.Items.Models;
using PokeGame.Core.Items.Payloads;
using PokeGame.Core.Models;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("api/items")]
  public class ItemApiController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IItemService _service;

    public ItemApiController(IMapper mapper, IItemService service)
    {
      _mapper = mapper;
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
      return Ok(await _service.DeleteAsync(id, cancellationToken));
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

      return Ok(new ListModel<ItemSummary>(_mapper.Map<IEnumerable<ItemSummary>>(items.Items), items.Total));
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
