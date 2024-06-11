using Microsoft.AspNetCore.Mvc;
using PokeGame.Application;
using PokeGame.Application.Accounts.Commands;
using PokeGame.Contracts.Accounts;
using PokeGame.Extensions;
using PokeGame.Models.Account;

namespace PokeGame.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
  private readonly IRequestPipeline _requestPipeline;

  public AccountController(IRequestPipeline requestPipeline)
  {
    _requestPipeline = requestPipeline;
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
}
