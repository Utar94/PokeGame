using Logitar.Portal.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("users")]
  public class UserController : Controller
  {
    private readonly PortalSettings _portalSettings;

    public UserController(PortalSettings portalSettings)
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
