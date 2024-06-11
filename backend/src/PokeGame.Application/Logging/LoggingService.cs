using Logitar.Portal.Contracts.ApiKeys;
using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;

namespace PokeGame.Application.Logging;

public class LoggingService : ILoggingService // TODO(fpion): Logging
{
  public virtual void SetApiKey(ApiKey? apiKey)
  {
  }

  public virtual void SetSession(Session? session)
  {
  }

  public virtual void SetUser(User? user)
  {
  }
}
