using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Portal.Contracts.Search;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts.Items;
using PokeGame.Domain.Items;

namespace PokeGame.Application.Items.Queries;

[Trait(Traits.Category, Categories.Integration)]
public class SearchItemsQueryTests : IntegrationTests
{
  private readonly IItemRepository _itemRepository;

  private readonly ItemAggregate _antidote;
  private readonly ItemAggregate _pokeBall;
  private readonly ItemAggregate _potion;
  private readonly ItemAggregate _revivalHerb;
  private readonly ItemAggregate _revive;

  public SearchItemsQueryTests() : base()
  {
    _itemRepository = ServiceProvider.GetRequiredService<IItemRepository>();

    IUniqueNameSettings uniqueNameSettings = ItemAggregate.UniqueNameSettings;
    _antidote = new(ItemCategory.Medicine, new UniqueNameUnit(uniqueNameSettings, "Antidote"));
    _pokeBall = new(ItemCategory.PokeBall, new UniqueNameUnit(uniqueNameSettings, "PokeBall"));
    _potion = new(ItemCategory.Medicine, new UniqueNameUnit(uniqueNameSettings, "Potion"));
    _revivalHerb = new(ItemCategory.Medicine, new UniqueNameUnit(uniqueNameSettings, "RevivalHerb"));
    _revive = new(ItemCategory.Medicine, new UniqueNameUnit(uniqueNameSettings, "Revive"));
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _itemRepository.SaveAsync([_antidote, _pokeBall, _potion, _revivalHerb, _revive]);
  }

  [Fact(DisplayName = "It should return empty search results.")]
  public async Task It_should_return_empty_search_results()
  {
    SearchItemsPayload payload = new()
    {
      Category = ItemCategory.BattleItem
    };

    SearchItemsQuery query = new(payload);
    SearchResults<Item> results = await Pipeline.ExecuteAsync(query);
    Assert.Equal(0, results.Total);
    Assert.Empty(results.Items);
  }

  [Fact(DisplayName = "It should return the correct search results.")]
  public async Task It_should_return_the_correct_search_results()
  {
    SearchItemsPayload payload = new()
    {
      Category = ItemCategory.Medicine,
      Ids = (await _itemRepository.LoadAsync()).Select(x => x.Id.ToGuid()).ToList(),
      Search = new TextSearch([new SearchTerm("po%"), new SearchTerm("re%")], SearchOperator.Or),
      Sort = [new ItemSortOption(ItemSort.UniqueName, isDescending: true)],
      Skip = 1,
      Limit = 1
    };

    payload.Ids.Remove(_revivalHerb.Id.ToGuid());
    payload.Ids.Add(Guid.Empty);

    SearchItemsQuery query = new(payload);
    SearchResults<Item> results = await Pipeline.ExecuteAsync(query);
    Assert.Equal(2, results.Total);
    Assert.Equal(payload.Limit, results.Items.Count);
    Assert.Equal(_potion.Id.ToGuid(), results.Items.Single().Id);
  }
}
