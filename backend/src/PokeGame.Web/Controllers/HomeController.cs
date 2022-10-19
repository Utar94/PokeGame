using Microsoft.AspNetCore.Mvc;
using PokeGame.Web.Configuration;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Route("")]
  public class HomeController : Controller
  {
    private readonly IConfigurationService _configurationService;

    public HomeController(IConfigurationService configurationService)
    {
      _configurationService = configurationService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
      if (await _configurationService.IsInitializedAsync(cancellationToken))
      {
        return RedirectToAction(actionName: "Index", controllerName: "Game");
      }

      return RedirectToAction("Startup");
    }

    [HttpGet("startup")]
    public async Task<IActionResult> Startup(CancellationToken cancellationToken)
    {
      if (await _configurationService.IsInitializedAsync(cancellationToken))
      {
        return RedirectToAction(actionName: "Index", controllerName: "Game");
      }

      return View();
    }
  }
}
