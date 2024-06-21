using FluentValidation.Results;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts.Items;
using PokeGame.Contracts.Items.Properties;
using PokeGame.Domain.Items;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.Application.Items.Commands;

[Trait(Traits.Category, Categories.Integration)]
public class CreateItemCommandTests : IntegrationTests
{
  private readonly IItemRepository _itemRepository;

  public CreateItemCommandTests() : base()
  {
    _itemRepository = ServiceProvider.GetRequiredService<IItemRepository>();
  }

  [Fact(DisplayName = "It should create a new item.")]
  public async Task It_should_create_a_new_item()
  {
    CreateItemPayload payload = new("Potion")
    {
      Category = ItemCategory.Medicine,
      Medicine = new MedicineProperties
      {
        HitPointHealing = 20
      }
    };
    CreateItemCommand command = new(payload);
    Item item = await Pipeline.ExecuteAsync(command);

    Assert.NotEqual(Guid.Empty, item.Id);
    Assert.Equal(2, item.Version);
    Assert.Equal(Actor, item.CreatedBy);
    Assert.Equal(Actor, item.UpdatedBy);
    Assert.True(item.CreatedOn < item.UpdatedOn);

    Assert.Equal(payload.Category, item.Category);
    Assert.Equal(payload.UniqueName, item.UniqueName);
    Assert.Equal(payload.Medicine, item.Medicine);

    ItemEntity? entity = await PokemonContext.Items.AsNoTracking().SingleOrDefaultAsync(x => x.AggregateId == new AggregateId(item.Id).Value);
    Assert.NotNull(entity);
  }

  [Fact(DisplayName = "It should throw UniqueNameAlreadyUsedException when the unique name is already used.")]
  public async Task It_should_throw_UniqueNameAlreadyUsedException_when_the_unique_name_is_already_used()
  {
    ItemAggregate item = new(ItemCategory.Medicine, new UniqueNameUnit(ItemAggregate.UniqueNameSettings, "Potion"));
    await _itemRepository.SaveAsync(item);

    CreateItemPayload payload = new(item.UniqueName.Value)
    {
      Category = ItemCategory.Medicine,
      Medicine = new MedicineProperties
      {
        HitPointHealing = 20
      }
    };
    CreateItemCommand command = new(payload);
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<ItemAggregate>>(async () => await Pipeline.ExecuteAsync(command));
    Assert.Equal(payload.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_is_not_valid()
  {
    CreateItemPayload payload = new("Potion")
    {
      Category = ItemCategory.Medicine,
      Medicine = new MedicineProperties
      {
        HitPointHealing = 20
      },
      PokeBall = new PokeBallProperties
      {
        CatchRateModifier = 1.5
      }
    };
    CreateItemCommand command = new(payload);
    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await Pipeline.ExecuteAsync(command));
    ValidationFailure error = Assert.Single(exception.Errors);
    Assert.Equal("NullValidator", error.ErrorCode);
    Assert.Equal("PokeBall", error.PropertyName);
  }
}
