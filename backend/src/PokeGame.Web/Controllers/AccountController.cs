using Logitar.Portal.Client;
using Logitar.Portal.Core.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Core;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Route("user")]
  public class AccountController : Controller
  {
    private readonly IAccountService _accountService;
    private readonly IUserContext _userContext;

    public AccountController(IAccountService accountService, IUserContext userContext)
    {
      _accountService = accountService;
      _userContext = userContext;
    }

    [Authorize(Policy = Constants.Policies.AuthenticatedUser)]
    [HttpGet("profile")]
    public async Task<ActionResult> Profile(CancellationToken cancellationToken)
    {
      UserModel user = await _accountService.GetProfileAsync(_userContext.SessionId, cancellationToken);

      return View(user);
    }

    [HttpGet("sign-in")]
    public IActionResult SignIn()
    {
      if (HttpContext.HasSession())
      {
        return RedirectToAction(actionName: "Profile");
      }

      return View();
    }

    [Authorize(Policy = Constants.Policies.AuthenticatedUser)]
    [HttpGet("sign-out")]
    public async Task<ActionResult> SignOut(CancellationToken cancellationToken)
    {
      await _accountService.SignOutAsync(_userContext.SessionId, cancellationToken);

      HttpContext.Session.Clear();
      HttpContext.Response.Cookies.Delete(Constants.Cookies.RenewToken);

      return RedirectToAction(actionName: "SignIn");
    }
  }
}
