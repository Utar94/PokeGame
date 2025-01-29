using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;

namespace PokeGame.Infrastructure.Actors;

public interface IActorService
{
  Task<IReadOnlyCollection<ActorModel>> FindAsync(IEnumerable<ActorId> ids, CancellationToken cancellationToken = default);
}
