using Logitar.Portal.Contracts.ApiKeys;
using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;

namespace PokeGame.Application.Logging;

public interface ILoggingService
{
  void SetApiKey(ApiKey? apiKey);
  void SetSession(Session? session);
  void SetUser(User? user);
}
