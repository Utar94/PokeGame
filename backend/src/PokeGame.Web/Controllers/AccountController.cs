using Logitar.Portal.Client;
using Logitar.Portal.Core.Tokens.Models;
using Logitar.Portal.Core.Tokens.Payloads;
using Logitar.Portal.Core.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Infrastructure;
using PokeGame.Web.Models.Users;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Route("user")]
  public class AccountController : Controller
  {
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;
    private readonly ITokenService _tokenService;
    private readonly IUserContext _userContext;

    public AccountController(
      ILogger<AccountController> logger,
      IAccountService accountService,
      ITokenService tokenService,
      IUserContext userContext
    )
    {
      _logger = logger;
      _accountService = accountService;
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
    public async Task<ActionResult> SignUp(string token, CancellationToken cancellationToken)
    {
      var validateTokenPayload = new ValidateTokenPayload
      {
        Purpose = Constants.CreateUser.Purpose,
        Realm = Constants.Realm,
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
