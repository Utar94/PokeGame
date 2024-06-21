using MediatR;
using PokeGame.Contracts.Items;

namespace PokeGame.Application.Items.Queries;

internal class ReadItemQueryHandler : IRequestHandler<ReadItemQuery, Item?>
{
  private readonly IItemQuerier _itemQuerier;

  public ReadItemQueryHandler(IItemQuerier itemQuerier)
  {
    _itemQuerier = itemQuerier;
  }

  public async Task<Item?> Handle(ReadItemQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, Item> items = new(capacity: 2);

    if (query.Id.HasValue)
    {
      Item? item = await _itemQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (item != null)
      {
        items[item.Id] = item;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.UniqueName))
    {
      Item? item = await _itemQuerier.ReadAsync(query.UniqueName, cancellationToken);
      if (item != null)
      {
        items[item.Id] = item;
      }
    }

    if (items.Count > 1)
    {
      throw TooManyResultsException<Item>.ExpectedSingle(items.Count);
    }

    return items.Values.SingleOrDefault();
  }
}
