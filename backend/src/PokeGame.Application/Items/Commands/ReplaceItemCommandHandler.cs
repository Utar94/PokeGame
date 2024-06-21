using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using MediatR;
using PokeGame.Application.Items.Validators;
using PokeGame.Contracts.Items;
using PokeGame.Domain;
using PokeGame.Domain.Items;
using PokeGame.Domain.Items.Properties;

namespace PokeGame.Application.Items.Commands;

internal class ReplaceItemCommandHandler : IRequestHandler<ReplaceItemCommand, Item?>
{
  private readonly IItemRepository _itemRepository;
  private readonly IItemQuerier _itemQuerier;
  private readonly ISender _sender;

  public ReplaceItemCommandHandler(IItemRepository itemRepository, IItemQuerier itemQuerier, ISender sender)
  {
    _itemRepository = itemRepository;
    _itemQuerier = itemQuerier;
    _sender = sender;
  }

  public async Task<Item?> Handle(ReplaceItemCommand command, CancellationToken cancellationToken)
  {
    IUniqueNameSettings uniqueNameSettings = ItemAggregate.UniqueNameSettings;

    ItemId id = new(command.Id);
    ItemAggregate? item = await _itemRepository.LoadAsync(id, cancellationToken);
    if (item == null)
    {
      return null;
    }
    ItemAggregate? reference = null;
    if (command.Version.HasValue)
    {
      reference = await _itemRepository.LoadAsync(item.Id, command.Version.Value, cancellationToken);
    }

    ReplaceItemPayload payload = command.Payload;
    new ReplaceItemValidator(uniqueNameSettings, item.Category).ValidateAndThrow(payload);

    if (reference == null || payload.Price != reference.Price)
    {
      item.Price = payload.Price;
    }

    UniqueNameUnit uniqueName = new(uniqueNameSettings, payload.UniqueName);
    if (reference == null || uniqueName != reference.UniqueName)
    {
      item.UniqueName = uniqueName;
    }
    DisplayNameUnit? displayName = DisplayNameUnit.TryCreate(payload.DisplayName);
    if (reference == null || displayName != reference.DisplayName)
    {
      item.DisplayName = displayName;
    }
    DescriptionUnit? description = DescriptionUnit.TryCreate(payload.Description);
    if (reference == null || description != reference.Description)
    {
      item.Description = description;
    }
    UrlUnit? picture = UrlUnit.TryCreate(payload.Picture);
    if (reference == null || picture != reference.Picture)
    {
      item.Picture = picture;
    }

    if (payload.Medicine != null)
    {
      ReadOnlyMedicineProperties properties = new(payload.Medicine);
      if (reference == null || properties != reference.Properties)
      {
        item.SetProperties(properties, command.ActorId);
      }
    }
    if (payload.PokeBall != null)
    {
      ReadOnlyPokeBallProperties properties = new(payload.PokeBall);
      if (reference == null || properties != reference.Properties)
      {
        item.SetProperties(properties, command.ActorId);
      }
    }

    UrlUnit? url = UrlUnit.TryCreate(payload.Reference);
    if (reference == null || url != reference.Reference)
    {
      item.Reference = url;
    }
    NotesUnit? notes = NotesUnit.TryCreate(payload.Notes);
    if (reference == null || notes != reference.Notes)
    {
      item.Notes = notes;
    }

    item.Update(command.ActorId);

    await _sender.Send(new SaveItemCommand(item), cancellationToken);

    return await _itemQuerier.ReadAsync(item, cancellationToken);
  }
}
