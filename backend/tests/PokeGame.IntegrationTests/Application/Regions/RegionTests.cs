using Logitar;
using Logitar.Portal.Contracts.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application.Regions.Commands;
using PokeGame.Application.Regions.Queries;
using PokeGame.Contracts;
using PokeGame.Contracts.Regions;
using PokeGame.Domain;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions;

[Trait(Traits.Category, Categories.Integration)]
public class RegionTests : IntegrationTests
{
  private readonly IRegionRepository _regionRepository;

  private readonly Region _kanto;

  public RegionTests() : base()
  {
    _regionRepository = ServiceProvider.GetRequiredService<IRegionRepository>();

    _kanto = new Region(new Name("Kanto"), UserId);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _regionRepository.SaveAsync(_kanto);
  }

  [Fact(DisplayName = "It should create a new region.")]
  public async Task It_should_create_a_new_region()
  {
    CreateOrReplaceRegionPayload payload = new(" Québec ")
    {
      Description = "    ",
      Link = "https://fr.wikipedia.org/wiki/Qu%C3%A9bec",
      Notes = "    "
    };
    CreateOrReplaceRegionCommand command = new(Guid.NewGuid(), payload, Version: null);
    CreateOrReplaceRegionResult result = await Pipeline.ExecuteAsync(command);
    Assert.True(result.Created);

    RegionModel? region = result.Region;
    Assert.NotNull(region);
    Assert.Equal(command.Id, region.Id);
    Assert.Equal(2, region.Version);
    Assert.Equal(DateTime.UtcNow, region.CreatedOn, TimeSpan.FromSeconds(1));
    Assert.True(region.CreatedOn < region.UpdatedOn);
    Assert.Equal(Actor, region.CreatedBy);
    Assert.Equal(Actor, region.UpdatedBy);

    Assert.Equal(payload.Name.Trim(), region.Name);
    Assert.Equal(payload.Description.CleanTrim(), region.Description);
    Assert.Equal(payload.Link, region.Link);
    Assert.Equal(payload.Notes.CleanTrim(), region.Notes);

    Assert.NotNull(await PokeGameContext.Regions.AsNoTracking().SingleOrDefaultAsync(x => x.Id == region.Id));
  }

  [Fact(DisplayName = "It should delete an existing region.")]
  public async Task It_should_delete_an_existing_region()
  {
    DeleteRegionCommand command = new(_kanto.Id.ToGuid());
    RegionModel? region = await Pipeline.ExecuteAsync(command);
    Assert.NotNull(region);
    Assert.Equal(command.Id, region.Id);

    Assert.Null(await PokeGameContext.Regions.AsNoTracking().SingleOrDefaultAsync(x => x.Id == region.Id));
  }

  [Fact(DisplayName = "It should replace an existing region.")]
  public async Task It_should_replace_an_existing_region()
  {
    long version = _kanto.Version;

    Description description = new("The Kanto region (Japanese: カントー地方 Kanto region) is a region of the Pokémon world. Kanto is located east of Johto, which together form a joint landmass that is south of Sinnoh.\n\nKanto is the setting of the first generation of games and can be explored in Generations II, III, IV, and VII.");
    _kanto.Description = description;
    _kanto.Update(UserId);
    await _regionRepository.SaveAsync(_kanto);

    CreateOrReplaceRegionPayload payload = new(" Kanto ")
    {
      Link = "https://bulbapedia.bulbagarden.net/wiki/Kanto",
      Notes = "    "
    };
    CreateOrReplaceRegionCommand command = new(_kanto.Id.ToGuid(), payload, version);
    CreateOrReplaceRegionResult result = await Pipeline.ExecuteAsync(command);
    Assert.False(result.Created);

    RegionModel? region = result.Region;
    Assert.NotNull(region);
    Assert.Equal(command.Id, region.Id);
    Assert.Equal(_kanto.Version + 1, region.Version);
    Assert.Equal(DateTime.UtcNow, region.UpdatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(Actor, region.UpdatedBy);

    Assert.Equal(payload.Name.Trim(), region.Name);
    Assert.Equal(description.Value, region.Description);
    Assert.Equal(payload.Link, region.Link);
    Assert.Equal(payload.Notes.CleanTrim(), region.Notes);
  }

  [Fact(DisplayName = "It should return empty search results.")]
  public async Task It_should_return_empty_search_results()
  {
    SearchRegionsPayload payload = new()
    {
      Search = new TextSearch([new SearchTerm("test")])
    };
    SearchRegionsQuery query = new(payload);
    SearchResults<RegionModel> results = await Pipeline.ExecuteAsync(query);
    Assert.Empty(results.Items);
    Assert.Equal(0, results.Total);
  }

  [Fact(DisplayName = "It should return the correct search results.")]
  public async Task It_should_return_the_correct_search_results()
  {
    Region johto = new(new Name("Johto"), UserId);
    Region hoenn = new(new Name("Hoenn"), UserId);
    Region sinnoh = new(new Name("Sinnoh"), UserId);
    await _regionRepository.SaveAsync([johto, hoenn, sinnoh]);

    SearchRegionsPayload payload = new()
    {
      Search = new TextSearch([new SearchTerm("%to"), new SearchTerm("ho%")], SearchOperator.Or),
      Sort = [new RegionSortOption(RegionSort.Name, isDescending: true)],
      Skip = 1,
      Limit = 1
    };

    payload.Ids.AddRange((await _regionRepository.LoadAsync()).Select(region => region.Id.ToGuid()));
    payload.Ids.Remove(_kanto.Id.ToGuid());
    payload.Ids.Add(Guid.Empty);

    SearchRegionsQuery query = new(payload);
    SearchResults<RegionModel> results = await Pipeline.ExecuteAsync(query);

    Assert.Equal(2, results.Total);
    Assert.Equal(hoenn.Id.ToGuid(), Assert.Single(results.Items).Id);
  }

  [Fact(DisplayName = "It should return the region found by ID.")]
  public async Task It_should_return_the_region_found_by_Id()
  {
    ReadRegionQuery query = new(_kanto.Id.ToGuid());
    RegionModel? region = await Pipeline.ExecuteAsync(query);
    Assert.NotNull(region);
    Assert.Equal(query.Id, region.Id);
  }

  [Fact(DisplayName = "It should update an existing region.")]
  public async Task It_should_update_an_existing_region()
  {
    UpdateRegionPayload payload = new()
    {
      Description = new Change<string>("  The Kanto region (Japanese: カントー地方 Kanto region) is a region of the Pokémon world. Kanto is located east of Johto, which together form a joint landmass that is south of Sinnoh.\n\nKanto is the setting of the first generation of games and can be explored in Generations II, III, IV, and VII.  "),
      Link = new Change<string>("https://bulbapedia.bulbagarden.net/wiki/Kanto")
    };
    UpdateRegionCommand command = new(_kanto.Id.ToGuid(), payload);
    RegionModel? region = await Pipeline.ExecuteAsync(command);
    Assert.NotNull(region);

    Assert.Equal(command.Id, region.Id);
    Assert.Equal(_kanto.Version + 1, region.Version);
    Assert.Equal(DateTime.UtcNow, region.UpdatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(Actor, region.UpdatedBy);

    Assert.Equal(_kanto.Name.Value, region.Name);
    Assert.Equal(payload.Description.Value?.CleanTrim(), region.Description);
    Assert.Equal(payload.Link.Value, region.Link);
    Assert.Equal(payload.Notes?.Value?.CleanTrim(), region.Notes);
  }
}
