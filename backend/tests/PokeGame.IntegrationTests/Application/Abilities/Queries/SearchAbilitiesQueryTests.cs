using Logitar.Identity.Contracts.Settings;
using Logitar.Portal.Contracts.Search;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Queries;

[Trait(Traits.Category, Categories.Integration)]
public class SearchAbilitiesQueryTests : IntegrationTests
{
  private readonly IAbilityRepository _abilityRepository;

  private readonly AbilityAggregate _adaptability;
  private readonly AbilityAggregate _blaze;
  private readonly AbilityAggregate _overgrow;
  private readonly AbilityAggregate _torrent;

  public SearchAbilitiesQueryTests() : base()
  {
    _abilityRepository = ServiceProvider.GetRequiredService<IAbilityRepository>();

    IUniqueNameSettings uniqueNameSettings = AbilityAggregate.UniqueNameSettings;
    _adaptability = new(new UniqueNameUnit(uniqueNameSettings, "Adaptability"));
    _blaze = new(new UniqueNameUnit(uniqueNameSettings, "Blaze"));
    _overgrow = new(new UniqueNameUnit(uniqueNameSettings, "Overgrow"));
    _torrent = new(new UniqueNameUnit(uniqueNameSettings, "Torrent"));
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _abilityRepository.SaveAsync([_adaptability, _blaze, _overgrow, _torrent]);
  }

  [Fact(DisplayName = "It should return empty search results.")]
  public async Task It_should_return_empty_search_results()
  {
    SearchAbilitiesPayload payload = new()
    {
      Search = new([new SearchTerm("%air%"), new SearchTerm("%lock%")])
    };

    SearchAbilitiesQuery query = new(payload);
    SearchResults<Ability> results = await Pipeline.ExecuteAsync(query);
    Assert.Equal(0, results.Total);
    Assert.Empty(results.Items);
  }

  [Fact(DisplayName = "It should return the correct search results.")]
  public async Task It_should_return_the_correct_search_results()
  {
    SearchAbilitiesPayload payload = new()
    {
      Ids = (await _abilityRepository.LoadAsync()).Select(x => x.Id.ToGuid()).ToList(),
      Search = new TextSearch([new SearchTerm("%o%"), new SearchTerm("___z_")], SearchOperator.Or),
      Sort = [new AbilitySortOption(AbilitySort.UniqueName, isDescending: false)],
      Skip = 1,
      Limit = 1
    };

    payload.Ids.Remove(_blaze.Id.ToGuid());
    payload.Ids.Add(Guid.Empty);

    SearchAbilitiesQuery query = new(payload);
    SearchResults<Ability> results = await Pipeline.ExecuteAsync(query);
    Assert.Equal(2, results.Total);
    Assert.Equal(payload.Limit, results.Items.Count);
    Assert.Equal(_torrent.Id.ToGuid(), results.Items.Single().Id);
  }
}
