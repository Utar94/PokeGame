using Logitar.Portal.Contracts.Realms;

namespace PokeGame.Application.Accounts;

public interface IRealmService
{
  Task<Realm> FindAsync(CancellationToken cancellationToken = default);
}
