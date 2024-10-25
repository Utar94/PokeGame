using Logitar.Portal.Contracts.Sessions;
using PokeGame.Contracts.Accounts;

namespace PokeGame.Authentication;

public interface IOpenAuthenticationService
{
  TokenResponse GetTokenResponse(Session session);
  ClaimsPrincipal ValidateToken(string token);
}
