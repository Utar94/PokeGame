using Logitar.Portal.Contracts.Sessions;
using PokeGame.Models.Account;

namespace PokeGame.Authentication;

public interface IOpenAuthenticationService
{
  Task<TokenResponse> GetTokenResponseAsync(Session session, CancellationToken cancellationToken = default);
  Task<Session> ValidateTokenAsync(string accessToken, CancellationToken cancellationToken = default);
}
