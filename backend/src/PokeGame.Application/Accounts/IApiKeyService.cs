using Logitar.Portal.Contracts.ApiKeys;

namespace PokeGame.Application.Accounts;

public interface IApiKeyService
{
  Task<ApiKey> AuthenticateAsync(string xApiKey, CancellationToken cancellationToken = default);
}
