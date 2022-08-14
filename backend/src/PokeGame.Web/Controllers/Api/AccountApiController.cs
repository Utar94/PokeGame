using Logitar.Portal.Client;
using Logitar.Portal.Core.Accounts.Payloads;
using Logitar.Portal.Core.Sessions.Models;
using Logitar.Portal.Core.Users.Models;
using Logitar.Portal.Core.Users.Payloads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Core;
using PokeGame.Web.Models.Api.Account;
using System.Text.Json;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Route("api/account")]
  public class AccountApiController : ControllerBase
  {
    private readonly IAccountService _accountService;
    private readonly IUserContext _userContext;

    public AccountApiController(IAccountService accountService, IUserContext userContext)
    {
      _accountService = accountService;
      _userContext = userContext;
    }

    [Authorize(Policy = Constants.Policies.AuthenticatedUser)]
    [HttpPost("password/change")]
    public async Task<ActionResult<UserModel>> ChangePasswordAsync([FromBody] ChangePasswordPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _accountService.ChangePasswordAsync(_userContext.SessionId, payload, cancellationToken));
    }

    [Authorize(Policy = Constants.Policies.AuthenticatedUser)]
    [HttpPut("profile")]
    public async Task<ActionResult<UserModel>> SaveProfileAsync([FromBody] SaveProfileModel model, CancellationToken cancellationToken)
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

      return Ok(await _accountService.SaveProfileAsync(_userContext.SessionId, payload, cancellationToken));
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
  }
}
