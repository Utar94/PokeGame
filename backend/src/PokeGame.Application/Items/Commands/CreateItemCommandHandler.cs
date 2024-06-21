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

internal class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Item>
{
  private readonly IItemQuerier _itemQuerier;
  private readonly ISender _sender;

  public CreateItemCommandHandler(IItemQuerier itemQuerier, ISender sender)
  {
    _itemQuerier = itemQuerier;
    _sender = sender;
  }

  public async Task<Item> Handle(CreateItemCommand command, CancellationToken cancellationToken)
  {
    IUniqueNameSettings uniqueNameSettings = ItemAggregate.UniqueNameSettings;

    CreateItemPayload payload = command.Payload;
    new CreateItemValidator(uniqueNameSettings, payload.Category).ValidateAndThrow(payload);

    ItemAggregate item = new(payload.Category, new UniqueNameUnit(uniqueNameSettings, payload.UniqueName), command.ActorId)
    {
      Price = payload.Price,
      DisplayName = DisplayNameUnit.TryCreate(payload.DisplayName),
      Description = DescriptionUnit.TryCreate(payload.Description),
      Picture = UrlUnit.TryCreate(payload.Picture),
      Reference = UrlUnit.TryCreate(payload.Reference),
      Notes = NotesUnit.TryCreate(payload.Notes)
    };
    item.Update(command.ActorId);

    if (payload.Medicine != null)
    {
      ReadOnlyMedicineProperties properties = new(payload.Medicine);
      item.SetProperties(properties, command.ActorId);
    }
    if (payload.PokeBall != null)
    {
      ReadOnlyPokeBallProperties properties = new(payload.PokeBall);
      item.SetProperties(properties, command.ActorId);
    }

    await _sender.Send(new SaveItemCommand(item), cancellationToken);

    return await _itemQuerier.ReadAsync(item, cancellationToken);
  }
}
