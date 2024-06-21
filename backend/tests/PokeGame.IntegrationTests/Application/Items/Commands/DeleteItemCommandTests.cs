using Logitar.Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts.Items;
using PokeGame.Domain.Items;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.Application.Items.Commands;

[Trait(Traits.Category, Categories.Integration)]
public class DeleteItemCommandTests : IntegrationTests
{
  private readonly IItemRepository _itemRepository;

  private readonly ItemAggregate _item;

  public DeleteItemCommandTests()
  {
    _itemRepository = ServiceProvider.GetRequiredService<IItemRepository>();

    _item = new(ItemCategory.Medicine, new UniqueNameUnit(ItemAggregate.UniqueNameSettings, "Potion"), ActorId);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _itemRepository.SaveAsync(_item);
  }

  [Fact(DisplayName = "It should delete an existing item.")]
  public async Task It_should_delete_an_existing_item()
  {
    DeleteItemCommand command = new(_item.Id.ToGuid());
    Item? item = await Pipeline.ExecuteAsync(command);
    Assert.NotNull(item);
    Assert.Equal(_item.Id.ToGuid(), item.Id);

    ItemEntity? entity = await PokemonContext.Items.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == _item.Id.Value);
    Assert.Null(entity);
  }

  [Fact(DisplayName = "It should return null when the item could not be found.")]
  public async Task It_should_return_null_when_the_item_could_not_be_found()
  {
    DeleteItemCommand command = new(Guid.NewGuid());
    Item? item = await Pipeline.ExecuteAsync(command);
    Assert.Null(item);
  }
}
