using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;

namespace PokeGame.Application;

public record ActivityContext(Session? Session, User? User);
