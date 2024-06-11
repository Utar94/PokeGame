using Logitar.Portal.Contracts.ApiKeys;
using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;

namespace PokeGame.Application;

public record ActivityContext(ApiKey? ApiKey, Session? Session, User? User);
