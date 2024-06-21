using MediatR;
using PokeGame.Contracts.Items;
using PokeGame.Domain.Items;

namespace PokeGame.Application.Items.Commands;

internal class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, Item?>
{
  private readonly IItemQuerier _itemQuerier;
  private readonly IItemRepository _itemRepository;

  public DeleteItemCommandHandler(IItemQuerier itemQuerier, IItemRepository itemRepository)
  {
    _itemQuerier = itemQuerier;
    _itemRepository = itemRepository;
  }

  public async Task<Item?> Handle(DeleteItemCommand command, CancellationToken cancellationToken)
  {
    ItemId id = new(command.Id);
    ItemAggregate? item = await _itemRepository.LoadAsync(id, cancellationToken);
    if (item == null)
    {
      return null;
    }
    Item result = await _itemQuerier.ReadAsync(item, cancellationToken);

    item.Delete(command.ActorId);

    await _itemRepository.SaveAsync(item, cancellationToken);

    return result;
  }
}
