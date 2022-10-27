using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Web.Settings;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("users")]
  public class UserController : Controller
  {
    private readonly ClientPortalSettings _portalSettings;

    public UserController(ClientPortalSettings portalSettings)
    {
      _portalSettings = portalSettings;
    }

    [HttpGet]
    public ActionResult UserList()
    {
      return View();
    }

    [HttpGet("invite")]
    public ActionResult InviteUser()
    {
      return View();
    }

    [HttpGet("{id}")]
    public ActionResult ViewUser(Guid id)
    {
      return Redirect($"{_portalSettings.BaseUrl}/users/{id}");
    }
  }
}
