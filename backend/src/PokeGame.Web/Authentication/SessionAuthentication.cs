using Logitar.Portal.Core.Sessions.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace PokeGame.Web.Authentication
{
  internal class SessionAuthenticationOptions : AuthenticationSchemeOptions
  {
  }

  internal class SessionAuthenticationHandler : AuthenticationHandler<SessionAuthenticationOptions>
  {
    public SessionAuthenticationHandler(
      IOptionsMonitor<SessionAuthenticationOptions> options,
      ILoggerFactory logger,
      UrlEncoder encoder,
      ISystemClock clock
    ) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
      SessionModel? session = Context.ReadSession();
      if (session != null)
      {
        AuthenticateResult? failure = null;
        if (!session.IsActive)
        {
          failure = AuthenticateResult.Fail($"The session 'Id={session.Id}' has ended.");
        }
        else if (session.User == null)
        {
          failure = AuthenticateResult.Fail($"The User was null for session 'Id={session.Id}'.");
        }
        else if (session.User.IsDisabled)
        {
          failure = AuthenticateResult.Fail($"The User is disabled for session 'Id={session.Id}'.");
        }

        if (failure != null)
        {
          Context.Session.Clear();

          return Task.FromResult(failure);
        }

        if (!Context.SetSession(session))
        {
          throw new InvalidOperationException("The Session context item could not be set.");
        }

        var principal = new ClaimsPrincipal(session.User!.GetClaimsIdentity(Constants.Schemes.Session));
        var ticket = new AuthenticationTicket(principal, Constants.Schemes.Session);

        return Task.FromResult(AuthenticateResult.Success(ticket));
      }

      return Task.FromResult(AuthenticateResult.NoResult());
    }
  }
}
