using Logitar.Net.Http;
using Logitar.Portal.Contracts.Errors;
using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application;
using PokeGame.Application.Accounts;
using PokeGame.Application.Accounts.Commands;
using PokeGame.Authentication;
using PokeGame.Contracts.Accounts;
using PokeGame.Contracts.Errors;
using PokeGame.Extensions;
using PokeGame.Models.Account;

namespace PokeGame.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
  private static readonly HashSet<string> _invalidCredentialCodes =
  [
    "IncorrectSessionSecret",
    "IncorrectUserPassword",
    "InvalidRefreshToken",
    "SessionIsNotActive",
    "SessionIsNotPersistent",
    "SessionNotFound",
    "UserHasNoPassword",
    "UserIsDisabled",
    "UserNotFound"
  ];
  private static readonly JsonSerializerOptions _serializerOptions = new();
  static AccountController()
  {
    _serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
  }

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
    try
    {
      SignInCommandResult result = await _requestPipeline.ExecuteAsync(new SignInCommand(payload, HttpContext.GetSessionCustomAttributes()), cancellationToken);
      if (result.Session != null)
      {
        HttpContext.SignIn(result.Session);
      }

      return Ok(new SignInResponse(result));
    }
    catch (Exception exception)
    {
      if (IsInvalidCredentialsException(exception))
      {
        return InvalidCredentials();
      }

      throw;
    }
  }

  [HttpPost("/auth/token")]
  public async Task<ActionResult<GetTokenResponse>> TokenAsync([FromBody] GetTokenPayload payload, CancellationToken cancellationToken)
  {
    try
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
    catch (Exception exception)
    {
      if (IsInvalidCredentialsException(exception))
      {
        return InvalidCredentials();
      }

      throw;
    }
  }

  [Authorize]
  [HttpPost("/auth/sign/out")]
  public async Task<ActionResult> SignOutAsync(bool everywhere, CancellationToken cancellationToken)
  {
    Session? session = HttpContext.GetSession();
    User? user = HttpContext.GetUser();
    if (everywhere && user != null)
    {
      SignOutCommand command = new(user);
      await _requestPipeline.ExecuteAsync(command, cancellationToken);
    }
    else if (!everywhere && session != null)
    {
      SignOutCommand command = new(session);
      await _requestPipeline.ExecuteAsync(command, cancellationToken);
    }

    HttpContext.SignOut();

    return NoContent();
  }

  [Authorize]
  [HttpGet("/profile")]
  public ActionResult<UserProfile> GetProfile()
  {
    User user = HttpContext.GetUser() ?? throw new InvalidOperationException("An authenticated user is required.");
    return Ok(user.ToUserProfile());
  }

  private static BadRequestObjectResult InvalidCredentials() => new(new InvalidCredentialsError());
  private static bool IsInvalidCredentialsException(Exception exception)
  {
    if (exception is HttpFailureException<JsonApiResult> failure && failure.Result.JsonContent != null)
    {
      Error? error = JsonSerializer.Deserialize<Error>(failure.Result.JsonContent, _serializerOptions);
      if (error != null && _invalidCredentialCodes.Contains(error.Code))
      {
        return true;
      }
    }

    return false;
  }
}
