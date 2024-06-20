using Logitar.EventSourcing;
using MediatR;
using PokeGame.Domain.Items;
using PokeGame.Domain.Items.Events;

namespace PokeGame.Application.Items.Commands;

internal class SaveItemCommandHandler : IRequestHandler<SaveItemCommand>
{
  private readonly IItemRepository _itemRepository;

  public SaveItemCommandHandler(IItemRepository itemRepository)
  {
    _itemRepository = itemRepository;
  }

  public async Task Handle(SaveItemCommand command, CancellationToken cancellationToken)
  {
    ItemAggregate item = command.Item;

    bool hasUniqueNameChanged = false;
    foreach (DomainEvent change in item.Changes)
    {
      if (change is ItemCreatedEvent || (change is ItemUpdatedEvent updated && updated.UniqueName != null))
      {
        hasUniqueNameChanged = true;
      }
    }

    if (hasUniqueNameChanged)
    {
      ItemAggregate? other = await _itemRepository.LoadAsync(item.UniqueName, cancellationToken);
      if (other != null && !other.Equals(item))
      {
        throw new UniqueNameAlreadyUsedException<ItemAggregate>(item.UniqueName, nameof(item.UniqueName));
      }
    }

    await _itemRepository.SaveAsync(item, cancellationToken);
  }
}
