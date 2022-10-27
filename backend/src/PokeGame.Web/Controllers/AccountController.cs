using Logitar.Portal.Client;
using Logitar.Portal.Core.Tokens.Models;
using Logitar.Portal.Core.Tokens.Payloads;
using Logitar.Portal.Core.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Infrastructure;
using PokeGame.Web.Models.Users;
using PokeGame.Web.Settings;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Route("user")]
  public class AccountController : Controller
  {
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;
    private readonly ClientPortalSettings _portalSettings;
    private readonly ITokenService _tokenService;
    private readonly IUserContext _userContext;

    public AccountController(
      IAccountService accountService,
      ILogger<AccountController> logger,
      ClientPortalSettings portalSettings,
      ITokenService tokenService,
      IUserContext userContext
    )
    {
      _accountService = accountService;
      _logger = logger;
      _portalSettings = portalSettings;
      _tokenService = tokenService;
      _userContext = userContext;
    }

    [Authorize(Policy = Constants.Policies.AuthenticatedUser)]
    [HttpGet("profile")]
    public async Task<ActionResult> Profile(CancellationToken cancellationToken)
    {
      UserModel user = await _accountService.GetProfileAsync(_userContext.SessionId, cancellationToken);

      return View(new ProfileModel(user));
    }

    [HttpGet("recover-password")]
    public IActionResult RecoverPassword()
    {
      return View();
    }

    [HttpGet("reset-password")]
    public IActionResult ResetPassword(string? token)
    {
      if (string.IsNullOrWhiteSpace(token))
      {
        return RedirectToAction(actionName: "SignIn");
      }

      return View(new ResetPasswordModel(token));
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

    [HttpGet("sign-up")]
    public async Task<ActionResult> SignUp(string? token, CancellationToken cancellationToken)
    {
      if (string.IsNullOrWhiteSpace(token))
      {
        return RedirectToAction(actionName: "SignIn");
      }

      var validateTokenPayload = new ValidateTokenPayload
      {
        Purpose = Constants.CreateUser.Purpose,
        Realm = _portalSettings.Realm,
        Token = token
      };
      ValidatedTokenModel validatedToken = await _tokenService.ValidateAsync(validateTokenPayload, cancellationToken);

      string email;
      try
      {
        validatedToken.EnsureHasSucceeded();
        email = validatedToken.Email ?? throw new InvalidOperationException($"The {nameof(validatedToken.Email)} is required.");
      }
      catch (Exception exception)
      {
        _logger.LogError(exception, "{message}", exception.Message);

        return RedirectToAction(actionName: "SignIn");
      }

      return View(new SignUpModel(email, token));
    }
  }
}
