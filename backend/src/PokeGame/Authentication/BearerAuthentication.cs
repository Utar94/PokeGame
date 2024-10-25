﻿using Logitar.Portal.Contracts.Constants;
using Logitar.Portal.Contracts.Users;
using Logitar.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using PokeGame.Application.Accounts;
using PokeGame.Constants;
using PokeGame.Extensions;

namespace PokeGame.Authentication;

internal class BearerAuthenticationOptions : AuthenticationSchemeOptions;

internal class BearerAuthenticationHandler : AuthenticationHandler<BearerAuthenticationOptions>
{
  private readonly IOpenAuthenticationService _openAuthenticationService;
  private readonly IUserService _userService;

  public BearerAuthenticationHandler(
    IOpenAuthenticationService openAuthenticationService,
    IUserService userService,
    IOptionsMonitor<BearerAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : base(options, logger, encoder)
  {
    _openAuthenticationService = openAuthenticationService;
    _userService = userService;
  }

  protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
  {
    if (Context.Request.Headers.TryGetValue(Headers.Authorization, out StringValues authorization))
    {
      string? value = authorization.Single();
      if (!string.IsNullOrWhiteSpace(value))
      {
        string[] values = value.Split();
        if (values.Length != 2)
        {
          return AuthenticateResult.Fail($"The Authorization header value is not valid: '{value}'.");
        }
        else if (values[0] == Schemes.Bearer)
        {
          try
          {
            ClaimsPrincipal principal = _openAuthenticationService.ValidateToken(values[1]);
            Claim[] subjects = principal.FindAll(Rfc7519ClaimNames.Subject).ToArray();
            if (subjects.Length < 1)
            {
              return AuthenticateResult.Fail($"The '{Rfc7519ClaimNames.Subject}' claim is required.");
            }
            else if (subjects.Length > 1)
            {
              return AuthenticateResult.Fail($"Only one '{Rfc7519ClaimNames.Subject}' claim value is supported.");
            }
            Claim subject = subjects[0];

            User? user = await _userService.FindAsync(Guid.Parse(subject.Value));
            if (user == null)
            {
              return AuthenticateResult.Fail($"The user 'Id={subject.Value}' could not be found.");
            }

            Context.SetUser(user);

            principal = new(user.CreateClaimsIdentity(Scheme.Name));
            AuthenticationTicket ticket = new(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
          }
          catch (Exception exception)
          {
            return AuthenticateResult.Fail(exception);
          }
        }
      }
    }

    return AuthenticateResult.NoResult();
  }
}
