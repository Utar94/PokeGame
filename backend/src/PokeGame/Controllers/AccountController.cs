﻿using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application;
using PokeGame.Application.Accounts;
using PokeGame.Application.Accounts.Commands;
using PokeGame.Authentication;
using PokeGame.Constants;
using PokeGame.Contracts.Accounts;
using PokeGame.Extensions;

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

    SignInResponse response = new(result);
    return Ok(response);
  }

  [HttpPost("/auth/sign/out")]
  [Authorize]
  public async Task<ActionResult> SignOutAsync(bool everywhere, CancellationToken cancellationToken)
  {
    if (everywhere)
    {
      User? user = HttpContext.GetUser();
      if (user != null)
      {
        await _requestPipeline.ExecuteAsync(SignOutCommand.User(user.Id), cancellationToken);
      }
    }
    else
    {
      Guid? sessionId = HttpContext.GetSessionId();
      if (sessionId.HasValue)
      {
        await _requestPipeline.ExecuteAsync(SignOutCommand.Session(sessionId.Value), cancellationToken);
      }
    }

    return NoContent();
  }

  [HttpPost("/auth/token")]
  public async Task<ActionResult<GetTokenResponse>> GetTokenAsync([FromBody] GetTokenPayload payload, CancellationToken cancellationToken)
  {
    GetTokenResponse response;
    Session? session;
    if (!string.IsNullOrWhiteSpace(payload.RefreshToken))
    {
      response = new GetTokenResponse();
      session = await _sessionService.RenewAsync(payload.RefreshToken.Trim(), HttpContext.GetSessionCustomAttributes(), cancellationToken);
    }
    else
    {
      SignInCommandResult result = await _requestPipeline.ExecuteAsync(new SignInCommand(payload, HttpContext.GetSessionCustomAttributes()), cancellationToken);
      response = new(result);
      session = result.Session;
    }

    if (session != null)
    {
      response.TokenResponse = _openAuthenticationService.GetTokenResponse(session);
    }

    return Ok(response);
  }

  [HttpPut("/password/change")]
  [Authorize(Policy = Policies.User)]
  public async Task<ActionResult<UserProfile>> ChangePasswordAsync([FromBody] ChangeAccountPasswordPayload payload, CancellationToken cancellationToken)
  {
    User user = await _requestPipeline.ExecuteAsync(new ChangePasswordCommand(payload), cancellationToken);
    return Ok(user.ToUserProfile());
  }

  [HttpPost("/password/reset")]
  public async Task<ActionResult<ResetPasswordResponse>> ResetPasswordAsync([FromBody] ResetPasswordPayload payload, CancellationToken cancellationToken)
  {
    ResetPasswordResult result = await _requestPipeline.ExecuteAsync(new ResetPasswordCommand(payload, HttpContext.GetSessionCustomAttributes()), cancellationToken);
    if (result.Session != null)
    {
      HttpContext.SignIn(result.Session);
    }

    return Ok(new ResetPasswordResponse(result));
  }

  [HttpPut("/phone/change")]
  [Authorize(Policy = Policies.User)]
  public async Task<ActionResult<ChangePhoneResult>> ChangePhoneAsync([FromBody] ChangePhonePayload payload, CancellationToken cancellationToken)
  {
    ChangePhoneResult result = await _requestPipeline.ExecuteAsync(new ChangePhoneCommand(payload), cancellationToken);
    return Ok(result);
  }

  [HttpPost("/phone/verify")]
  public async Task<ActionResult<VerifyPhoneResult>> VerifyPhoneAsync([FromBody] VerifyPhonePayload payload, CancellationToken cancellationToken)
  {
    VerifyPhoneResult result = await _requestPipeline.ExecuteAsync(new VerifyPhoneCommand(payload), cancellationToken);
    return Ok(result);
  }

  [HttpGet("/profile")]
  [Authorize(Policy = Policies.User)]
  public ActionResult<UserProfile> GetProfile()
  {
    User user = HttpContext.GetUser() ?? throw new InvalidOperationException("An authenticated user is required.");
    return Ok(user.ToUserProfile());
  }

  [HttpPut("/profile")]
  [Authorize(Policy = Policies.User)]
  public async Task<ActionResult<UserProfile>> SaveProfileAsync([FromBody] SaveProfilePayload payload, CancellationToken cancellationToken)
  {
    User user = await _requestPipeline.ExecuteAsync(new SaveProfileCommand(payload), cancellationToken);
    return Ok(user.ToUserProfile());
  }
}
