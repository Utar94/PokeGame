using Logitar.Identity.Domain.Shared;
using PokeGame.Domain.Items;

namespace PokeGame.Application.Items;

public interface IItemRepository
{
  Task<IReadOnlyCollection<ItemAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  Task<ItemAggregate?> LoadAsync(ItemId id, CancellationToken cancellationToken = default);
  Task<ItemAggregate?> LoadAsync(ItemId id, long? version, CancellationToken cancellationToken = default);
  Task<ItemAggregate?> LoadAsync(UniqueNameUnit uniqueName, CancellationToken cancellationToken = default);

  Task SaveAsync(ItemAggregate item, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<ItemAggregate> items, CancellationToken cancellationToken = default);
}
