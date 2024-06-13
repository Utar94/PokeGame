using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application;
using PokeGame.Application.Accounts;
using PokeGame.Application.Accounts.Commands;
using PokeGame.Authentication;
using PokeGame.Contracts.Accounts;
using PokeGame.Extensions;
using PokeGame.Models.Account;

namespace PokeGame.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
  private readonly IOpenAuthenticationService _openAuthenticationService;
  private readonly IRequestPipeline _requestPipeline;
  private readonly ISessionService _sessionService;

  public AccountController(IOpenAuthenticationService openAuthenticationService, IRequestPipeline requestPipeline, ISessionService sessionService)
  {
    _openAuthenticationService = openAuthenticationService;
    _requestPipeline = requestPipeline;
    _sessionService = sessionService;
  }

  [HttpPost("/auth/sign/in")]
  public async Task<ActionResult<SignInResponse>> SignInAsync([FromBody] SignInPayload payload, CancellationToken cancellationToken)
  {
    SignInCommandResult result = await _requestPipeline.ExecuteAsync(new SignInCommand(payload, HttpContext.GetSessionCustomAttributes()), cancellationToken);
    if (result.Session != null)
    {
      HttpContext.SignIn(result.Session);
    }

    return Ok(new SignInResponse(result));
  }

  [HttpPost("/auth/token")]
  public async Task<ActionResult<GetTokenResponse>> TokenAsync([FromBody] GetTokenPayload payload, CancellationToken cancellationToken)
  {
    GetTokenResponse response;
    Session? session;
    if (!string.IsNullOrWhiteSpace(payload.RefreshToken))
    {
      response = new GetTokenResponse();
      session = await _sessionService.RenewAsync(payload.RefreshToken, HttpContext.GetSessionCustomAttributes(), cancellationToken);
    }
    else
    {
      SignInCommandResult result = await _requestPipeline.ExecuteAsync(new SignInCommand(payload, HttpContext.GetSessionCustomAttributes()), cancellationToken);
      response = new(result);
      session = result.Session;
    }

    if (session != null)
    {
      response.TokenResponse = await _openAuthenticationService.GetTokenResponseAsync(session, cancellationToken);
    }

    return Ok(response);
  }

  [Authorize]
  [HttpGet("/profile")]
  public ActionResult<UserProfile> GetProfile()
  {
    User user = HttpContext.GetUser() ?? throw new InvalidOperationException("An authenticated user is required.");
    return Ok(user.ToUserProfile());
  }
}
