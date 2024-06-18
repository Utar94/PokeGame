using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts.Regions;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Queries;

[Trait(Traits.Category, Categories.Integration)]
public class ReadRegionQueryTests : IntegrationTests
{
  private readonly IRegionRepository _regionRepository;

  private readonly RegionAggregate _kanto;
  private readonly RegionAggregate _johto;

  public ReadRegionQueryTests() : base()
  {
    _regionRepository = ServiceProvider.GetRequiredService<IRegionRepository>();

    IUniqueNameSettings uniqueNameSettings = RegionAggregate.UniqueNameSettings;
    _kanto = new(new UniqueNameUnit(uniqueNameSettings, "Kanto"));
    _johto = new(new UniqueNameUnit(uniqueNameSettings, "Johto"));
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _regionRepository.SaveAsync([_kanto, _johto]);
  }

  [Fact(DisplayName = "It should return null when the region is not found.")]
  public async Task It_should_return_null_when_the_region_is_not_found()
  {
    ReadRegionQuery query = new(Id: Guid.NewGuid(), UniqueName: "Leafage");
    Assert.Null(await Pipeline.ExecuteAsync(query));
  }

  [Fact(DisplayName = "It should return the region found by ID.")]
  public async Task It_should_return_the_region_found_by_Id()
  {
    ReadRegionQuery query = new(_kanto.Id.ToGuid(), UniqueName: null);
    Region? region = await Pipeline.ExecuteAsync(query);
    Assert.NotNull(region);
    Assert.Equal(_kanto.Id.ToGuid(), region.Id);
  }

  [Fact(DisplayName = "It should return the region found by unique name.")]
  public async Task It_should_return_the_region_found_by_unique_name()
  {
    ReadRegionQuery query = new(Id: null, _johto.UniqueName.Value);
    Region? region = await Pipeline.ExecuteAsync(query);
    Assert.NotNull(region);
    Assert.Equal(_johto.Id.ToGuid(), region.Id);
  }

  [Fact(DisplayName = "It should throw TooManyResultsException when many regions are found.")]
  public async Task It_should_throw_TooManyResultsException_when_many_regions_are_found()
  {
    ReadRegionQuery query = new(_kanto.Id.ToGuid(), _johto.UniqueName.Value);
    var exception = await Assert.ThrowsAsync<TooManyResultsException<Region>>(async () => await Pipeline.ExecuteAsync(query));
    Assert.Equal(1, exception.ExpectedCount);
    Assert.Equal(2, exception.ActualCount);
  }
}
