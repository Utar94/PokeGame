using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Portal.Contracts.Search;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts.Regions;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Queries;

[Trait(Traits.Category, Categories.Integration)]
public class SearchRegionsQueryTests : IntegrationTests
{
  private readonly IRegionRepository _regionRepository;

  private readonly RegionAggregate _kanto;
  private readonly RegionAggregate _johto;
  private readonly RegionAggregate _hoenn;
  private readonly RegionAggregate _sinnoh;

  public SearchRegionsQueryTests() : base()
  {
    _regionRepository = ServiceProvider.GetRequiredService<IRegionRepository>();

    IUniqueNameSettings uniqueNameSettings = RegionAggregate.UniqueNameSettings;
    _kanto = new(new UniqueNameUnit(uniqueNameSettings, "Kanto"));
    _johto = new(new UniqueNameUnit(uniqueNameSettings, "Johto"));
    _hoenn = new(new UniqueNameUnit(uniqueNameSettings, "Hoenn"));
    _sinnoh = new(new UniqueNameUnit(uniqueNameSettings, "Sinnoh"));
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _regionRepository.SaveAsync([_kanto, _johto, _hoenn, _sinnoh]);
  }

  [Fact(DisplayName = "It should return empty search results.")]
  public async Task It_should_return_empty_search_results()
  {
    SearchRegionsPayload payload = new()
    {
      Search = new TextSearch([new SearchTerm("Unova")])
    };

    SearchRegionsQuery query = new(payload);
    SearchResults<Region> results = await Pipeline.ExecuteAsync(query);
    Assert.Equal(0, results.Total);
    Assert.Empty(results.Items);
  }

  [Fact(DisplayName = "It should return the correct search results.")]
  public async Task It_should_return_the_correct_search_results()
  {
    SearchRegionsPayload payload = new()
    {
      Ids = (await _regionRepository.LoadAsync()).Select(x => x.Id.ToGuid()).ToList(),
      Search = new TextSearch([new SearchTerm("%to"), new SearchTerm("%oh")], SearchOperator.Or),
      Sort = [new RegionSortOption(RegionSort.UniqueName, isDescending: false)],
      Skip = 1,
      Limit = 1
    };

    payload.Ids.Remove(_sinnoh.Id.ToGuid());
    payload.Ids.Add(Guid.Empty);

    SearchRegionsQuery query = new(payload);
    SearchResults<Region> results = await Pipeline.ExecuteAsync(query);
    Assert.Equal(2, results.Total);
    Assert.Equal(payload.Limit, results.Items.Count);
    Assert.Equal(_kanto.Id.ToGuid(), results.Items.Single().Id);
  }
}
