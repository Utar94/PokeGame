using FluentValidation.Results;
using Logitar;
using Logitar.Identity.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts.Items;
using PokeGame.Contracts.Items.Properties;
using PokeGame.Domain.Items;

namespace PokeGame.Application.Items.Commands;

[Trait(Traits.Category, Categories.Integration)]
public class ReplaceItemCommandTests : IntegrationTests
{
  private readonly IItemRepository _itemRepository;

  private readonly ItemAggregate _item;

  public ReplaceItemCommandTests() : base()
  {
    _itemRepository = ServiceProvider.GetRequiredService<IItemRepository>();

    _item = new(ItemCategory.Medicine, new UniqueNameUnit(ItemAggregate.UniqueNameSettings, "Potion"), ActorId);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _itemRepository.SaveAsync(_item);
  }

  [Fact(DisplayName = "It should replace an existing item.")]
  public async Task It_should_replace_an_existing_item()
  {
    long version = _item.Version;

    _item.Description = new DescriptionUnit("A spray-type medicine for treating wounds. It can be used to restore 20 HP to a Pokémon.");
    _item.Update(ActorId);
    await _itemRepository.SaveAsync(_item);

    ReplaceItemPayload payload = new("Potion")
    {
      Price = 200,
      DisplayName = "  Potion  ",
      Description = "    ",
      Picture = "https://bulbapedia.bulbagarden.net/wiki/File:Potion_SV.png",
      Medicine = new MedicineProperties
      {
        HitPointHealing = 20
      },
      Reference = "https://bulbapedia.bulbagarden.net/wiki/Potion",
      Notes = "    "
    };
    ReplaceItemCommand command = new(_item.Id.ToGuid(), payload, version);
    Item? item = await Pipeline.ExecuteAsync(command);
    Assert.NotNull(item);

    Assert.Equal(_item.Id.ToGuid(), item.Id);
    Assert.Equal(_item.Version + 2, item.Version);
    Assert.Equal(Actor, item.CreatedBy);
    Assert.Equal(Actor, item.UpdatedBy);
    Assert.True(item.CreatedOn < item.UpdatedOn);

    Assert.Equal(payload.Price, item.Price);
    Assert.Equal(payload.UniqueName, item.UniqueName);
    Assert.Equal(payload.DisplayName.CleanTrim(), item.DisplayName);
    Assert.Equal(_item.Description.Value, item.Description);
    Assert.Equal(payload.Picture, item.Picture);
    Assert.Equal(payload.Medicine, item.Medicine);
    Assert.Equal(payload.Reference.CleanTrim(), item.Reference);
    Assert.Equal(payload.Notes.CleanTrim(), item.Notes);
  }

  [Fact(DisplayName = "It should return null when the item could not be found.")]
  public async Task It_should_return_null_when_the_item_could_not_be_found()
  {
    ReplaceItemPayload payload = new("Potion")
    {
      Medicine = new MedicineProperties
      {
        HitPointHealing = 20
      }
    };
    ReplaceItemCommand command = new(Id: Guid.NewGuid(), payload, Version: null);
    Assert.Null(await Pipeline.ExecuteAsync(command));
  }

  [Fact(DisplayName = "It should throw UniqueNameAlreadyUsedException when the unique name is already used.")]
  public async Task It_should_throw_UniqueNameAlreadyUsedException_when_the_unique_name_is_already_used()
  {
    ItemAggregate item = new(ItemCategory.Medicine, new UniqueNameUnit(ItemAggregate.UniqueNameSettings, "Antidote"), ActorId);
    await _itemRepository.SaveAsync(item);

    ReplaceItemPayload payload = new(item.UniqueName.Value)
    {
      Medicine = new MedicineProperties
      {
        RemoveStatusCondition = "Poison"
      }
    };
    ReplaceItemCommand command = new(_item.Id.ToGuid(), payload, Version: null);
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<ItemAggregate>>(async () => await Pipeline.ExecuteAsync(command));
    Assert.Equal(payload.UniqueName, exception.UniqueName);
    Assert.Equal(nameof(payload.UniqueName), exception.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_is_not_valid()
  {
    ReplaceItemPayload payload = new("Potion")
    {
      Price = -200,
      Medicine = new MedicineProperties
      {
        HitPointHealing = 20
      }
    };
    ReplaceItemCommand command = new(_item.Id.ToGuid(), payload, Version: null);
    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await Pipeline.ExecuteAsync(command));
    ValidationFailure error = Assert.Single(exception.Errors);
    Assert.Equal("GreaterThanValidator", error.ErrorCode);
    Assert.Equal("Price.Value", error.PropertyName);
  }
}
