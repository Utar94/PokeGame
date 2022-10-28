using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Web.Settings;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("api-keys")]
  public class ApiKeyController : Controller
  {
    private readonly ClientPortalSettings _portalSettings;

    public ApiKeyController(ClientPortalSettings portalSettings)
    {
      _portalSettings = portalSettings;
    }

    [HttpGet("{id}")]
    public ActionResult ViewApiKey(Guid id)
    {
      return Redirect($"{_portalSettings.BaseUrl}/api-keys/{id}");
    }
  }
}
