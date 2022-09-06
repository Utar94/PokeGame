using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Items;
using PokeGame.Application.Items.Models;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("items")]
  public class ItemController : Controller
  {
    private readonly IItemService _itemService;

    public ItemController(IItemService itemService)
    {
      _itemService = itemService;
    }

    [HttpGet("/create-item")]
    public ActionResult CreateItem()
    {
      return View(nameof(ItemEdit));
    }

    [HttpGet]
    public ActionResult ItemList()
    {
      return View();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> ItemEdit(Guid id, CancellationToken cancellationToken = default)
    {
      ItemModel? item = await _itemService.GetAsync(id, cancellationToken);
      if (item == null)
      {
        return NotFound();
      }

      return View(item);
    }
  }
}
