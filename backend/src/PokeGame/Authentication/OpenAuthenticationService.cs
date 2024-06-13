using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Tokens;
using Logitar.Portal.Contracts.Users;
using Logitar.Security.Claims;
using PokeGame.Application.Accounts;
using PokeGame.Constants;
using PokeGame.Models.Account;
using PokeGame.Settings;

namespace PokeGame.Authentication;

internal class OpenAuthenticationService : IOpenAuthenticationService
{
  private readonly OpenAuthenticationSettings _settings;
  private readonly ISessionClient _sessionClient;
  private readonly ITokenClient _tokenClient;

  public OpenAuthenticationService(OpenAuthenticationSettings settings, ISessionClient sessionClient, ITokenClient tokenClient)
  {
    _settings = settings;
    _sessionClient = sessionClient;
    _tokenClient = tokenClient;
  }

  public virtual async Task<TokenResponse> GetTokenResponseAsync(Session session, CancellationToken cancellationToken)
  {
    CreateTokenPayload payload = BuildCreateTokenPayload(session);
    RequestContext context = new(cancellationToken);
    CreatedToken access = await _tokenClient.CreateAsync(payload, context);

    TokenResponse tokenResponse = new(access.Token, Schemes.Bearer)
    {
      ExpiresIn = _settings.AccessToken.Lifetime,
      RefreshToken = session.RefreshToken
    };
    return tokenResponse;
  }
  private CreateTokenPayload BuildCreateTokenPayload(Session session)
  {
    User user = session.User;

    CreateTokenPayload payload = new()
    {
      LifetimeSeconds = _settings.AccessToken.Lifetime,
      Type = _settings.AccessToken.TokenType,
      Subject = user.GetSubject(),
    };

    payload.Claims.Add(new TokenClaim(Rfc7519ClaimNames.SessionId, session.Id.ToString()));

    payload.Claims.Add(new TokenClaim(Rfc7519ClaimNames.Username, user.UniqueName));
    if (user.FullName != null)
    {
      payload.Claims.Add(new TokenClaim(Rfc7519ClaimNames.Username, user.FullName));

      if (user.FirstName != null)
      {
        payload.Claims.Add(new TokenClaim(Rfc7519ClaimNames.FirstName, user.FirstName));
      }
      if (user.MiddleName != null)
      {
        payload.Claims.Add(new TokenClaim(Rfc7519ClaimNames.MiddleName, user.MiddleName));
      }
      if (user.LastName != null)
      {
        payload.Claims.Add(new TokenClaim(Rfc7519ClaimNames.LastName, user.LastName));
      }
    }
    if (user.Email != null)
    {
      payload.Email = new EmailPayload(user.Email.Address, user.Email.IsVerified);
    }
    if (user.Picture != null)
    {
      payload.Claims.Add(new TokenClaim(Rfc7519ClaimNames.Picture, user.Picture));
    }

    return payload;
  }

  public async Task<Session> ValidateTokenAsync(string accessToken, CancellationToken cancellationToken)
  {
    ValidateTokenPayload payload = new(accessToken)
    {
      Type = _settings.AccessToken.TokenType
    };
    RequestContext context = new(cancellationToken);
    ValidatedToken validatedToken = await _tokenClient.ValidateAsync(payload, context);

    TokenClaim claim = validatedToken.Claims.Single(claim => claim.Name == Rfc7519ClaimNames.SessionId);
    Guid sessionId = Guid.Parse(claim.Value);

    Session? session = await _sessionClient.ReadAsync(sessionId, context);
    return session ?? throw new InvalidOperationException($"The session 'Id={sessionId}' could not be found.");
  }
}
