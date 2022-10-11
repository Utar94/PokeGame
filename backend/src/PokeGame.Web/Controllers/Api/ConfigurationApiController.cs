using Logitar.Portal.Client;
using Logitar.Portal.Core.Accounts.Payloads;
using Logitar.Portal.Core.Sessions.Models;
using Logitar.Portal.Core.Users.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PokeGame.ReadModel.Handlers.Users;
using PokeGame.Web.Configuration;
using PokeGame.Web.Models.Api.Configuration;
using System.Text.Json;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Route("api/configurations")]
  public class ConfigurationApiController : ControllerBase
  {
    private readonly IAccountService _accountService;
    private readonly IConfigurationService _configurationService;
    private readonly IMediator _mediator;

    public ConfigurationApiController(
      IAccountService accountService,
      IConfigurationService configurationService,
      IMediator mediator
    )
    {
      _accountService = accountService;
      _configurationService = configurationService;
      _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> InitializeAsync([FromBody] InitializeConfigurationModel payload, CancellationToken cancellationToken)
    {
      UserModel user = await _configurationService.InitializeAsync(payload, cancellationToken);
      await _mediator.Publish(new SaveUser(user), cancellationToken);

      var signInPayload = new SignInPayload
      {
        Username = payload.User.Username,
        Password = payload.User.Password,
        AdditionalInformation = JsonSerializer.Serialize(Request.Headers),
        IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
      };
      SessionModel session = await _accountService.SignInAsync(Constants.Realm, signInPayload, cancellationToken);
      HttpContext.SignIn(session);

      return NoContent();
    }
  }
}
