using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts.Items;
using PokeGame.Domain.Items;

namespace PokeGame.Application.Items.Queries;

[Trait(Traits.Category, Categories.Integration)]
public class ReadItemQueryTests : IntegrationTests
{
  private readonly IItemRepository _itemRepository;

  private readonly ItemAggregate _pokeBall;
  private readonly ItemAggregate _potion;

  public ReadItemQueryTests() : base()
  {
    _itemRepository = ServiceProvider.GetRequiredService<IItemRepository>();

    IUniqueNameSettings uniqueNameSettings = ItemAggregate.UniqueNameSettings;
    _pokeBall = new(ItemCategory.PokeBall, new UniqueNameUnit(uniqueNameSettings, "PokeBall"));
    _potion = new(ItemCategory.Medicine, new UniqueNameUnit(uniqueNameSettings, "Potion"));
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _itemRepository.SaveAsync([_pokeBall, _potion]);
  }

  [Fact(DisplayName = "It should return null when the item is not found.")]
  public async Task It_should_return_null_when_the_item_is_not_found()
  {
    ReadItemQuery query = new(Id: Guid.NewGuid(), UniqueName: "Leafage");
    Assert.Null(await Pipeline.ExecuteAsync(query));
  }

  [Fact(DisplayName = "It should return the item found by ID.")]
  public async Task It_should_return_the_item_found_by_Id()
  {
    ReadItemQuery query = new(_pokeBall.Id.ToGuid(), UniqueName: null);
    Item? item = await Pipeline.ExecuteAsync(query);
    Assert.NotNull(item);
    Assert.Equal(_pokeBall.Id.ToGuid(), item.Id);
  }

  [Fact(DisplayName = "It should return the item found by unique name.")]
  public async Task It_should_return_the_item_found_by_unique_name()
  {
    ReadItemQuery query = new(Id: null, _potion.UniqueName.Value);
    Item? item = await Pipeline.ExecuteAsync(query);
    Assert.NotNull(item);
    Assert.Equal(_potion.Id.ToGuid(), item.Id);
  }

  [Fact(DisplayName = "It should throw TooManyResultsException when many items are found.")]
  public async Task It_should_throw_TooManyResultsException_when_many_items_are_found()
  {
    ReadItemQuery query = new(_pokeBall.Id.ToGuid(), _potion.UniqueName.Value);
    var exception = await Assert.ThrowsAsync<TooManyResultsException<Item>>(async () => await Pipeline.ExecuteAsync(query));
    Assert.Equal(1, exception.ExpectedCount);
    Assert.Equal(2, exception.ActualCount);
  }
}
