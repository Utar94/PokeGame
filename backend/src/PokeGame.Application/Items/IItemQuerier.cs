using Logitar.Portal.Contracts.Search;
using PokeGame.Contracts.Items;
using PokeGame.Domain.Items;

namespace PokeGame.Application.Items;

public interface IItemQuerier
{
  Task<Item> ReadAsync(ItemAggregate item, CancellationToken cancellationToken = default);
  Task<Item?> ReadAsync(ItemId id, CancellationToken cancellationToken = default);
  Task<Item?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<Item?> ReadAsync(string uniqueName, CancellationToken cancellationToken = default);

  Task<SearchResults<Item>> SearchAsync(SearchItemsPayload payload, CancellationToken cancellationToken = default);
}
