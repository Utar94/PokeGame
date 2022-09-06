using Logitar.Portal.Client;
using Logitar.Portal.Core.Accounts.Payloads;
using Logitar.Portal.Core.Sessions.Models;
using Logitar.Portal.Core.Tokens.Models;
using Logitar.Portal.Core.Tokens.Payloads;
using Logitar.Portal.Core.Users.Models;
using Logitar.Portal.Core.Users.Payloads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Infrastructure;
using PokeGame.Web.Models.Api.Account;
using PokeGame.Web.Models.Users;
using System.Text.Json;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Route("api/account")]
  public class AccountApiController : ControllerBase
  {
    private readonly IAccountService _accountService;
    private readonly ITokenService _tokenService;
    private readonly IUserContext _userContext;
    private readonly IUserService _userService;

    public AccountApiController(
      IAccountService accountService,
      ITokenService tokenService,
      IUserContext userContext,
      IUserService userService
    )
    {
      _accountService = accountService;
      _tokenService = tokenService;
      _userContext = userContext;
      _userService = userService;
    }

    [Authorize(Policy = Constants.Policies.AuthenticatedUser)]
    [HttpPost("password/change")]
    public async Task<ActionResult<ProfileModel>> ChangePasswordAsync([FromBody] ChangePasswordPayload payload, CancellationToken cancellationToken)
    {
      UserModel user = await _accountService.ChangePasswordAsync(_userContext.SessionId, payload, cancellationToken);

      return Ok(new ProfileModel(user));
    }

    [Authorize(Policy = Constants.Policies.AuthenticatedUser)]
    [HttpPut("profile")]
    public async Task<ActionResult<ProfileModel>> SaveProfileAsync([FromBody] SaveProfileModel model, CancellationToken cancellationToken)
    {
      UserModel user = HttpContext.GetUser() ?? throw new InvalidOperationException("The User is required");

      var payload = new UpdateUserPayload
      {
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        FirstName = model.FirstName,
        LastName = model.LastName,
        MiddleName = model.MiddleName,
        Locale = model.Locale,
        Picture = model.Picture
      };
      user = await _accountService.SaveProfileAsync(_userContext.SessionId, payload, cancellationToken);

      return Ok(new ProfileModel(user));
    }

    [HttpPost("sign/in")]
    public async Task<ActionResult> SignInAsync([FromBody] SignInModel model, CancellationToken cancellationToken)
    {
      var payload = new SignInPayload
      {
        Username = model.Username,
        Password = model.Password,
        Remember = model.Remember,
        AdditionalInformation = JsonSerializer.Serialize(Request.Headers),
        IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
      };
      SessionModel session = await _accountService.SignInAsync(Constants.Realm, payload, cancellationToken);
      HttpContext.SignIn(session);

      return NoContent();
    }

    [HttpPost("sign/up")]
    public async Task<ActionResult> CreateAsync([FromBody] SignUpPayload payload, CancellationToken cancellationToken)
    {
      var validateTokenPayload = new ValidateTokenPayload
      {
        Purpose = Constants.CreateUser.Purpose,
        Realm = Constants.Realm,
        Token = payload.Token
      };
      ValidatedTokenModel validatedToken = await _tokenService.ConsumeAsync(validateTokenPayload, cancellationToken);
      validatedToken.EnsureHasSucceeded();
      string email = validatedToken.Email ?? throw new InvalidOperationException($"The {nameof(validatedToken.Email)} is required.");

      var createUserPayload = new CreateUserPayload
      {
        Realm = Constants.Realm,
        Username = email,
        Password = payload.Password,
        Email = validatedToken.Email,
        ConfirmEmail = true,
        FirstName = payload.FirstName,
        LastName = payload.LastName,
        Locale = payload.Locale
      };
      await _userService.CreateAsync(createUserPayload, cancellationToken);

      var signInPayload = new SignInPayload
      {
        Username = email,
        Password = payload.Password,
        AdditionalInformation = JsonSerializer.Serialize(Request.Headers),
        IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
      };
      SessionModel session = await _accountService.SignInAsync(Constants.Realm, signInPayload, cancellationToken);
      HttpContext.SignIn(session);

      return NoContent();
    }
  }
}
