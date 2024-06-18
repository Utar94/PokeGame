using Logitar.Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts.Regions;
using PokeGame.Domain.Regions;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.Application.Regions.Commands;

[Trait(Traits.Category, Categories.Integration)]
public class DeleteRegionCommandTests : IntegrationTests
{
  private readonly IRegionRepository _regionRepository;

  private readonly RegionAggregate _region;

  public DeleteRegionCommandTests()
  {
    _regionRepository = ServiceProvider.GetRequiredService<IRegionRepository>();

    _region = new(new UniqueNameUnit(RegionAggregate.UniqueNameSettings, "Kanto"), ActorId);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _regionRepository.SaveAsync(_region);
  }

  [Fact(DisplayName = "It should delete an existing region.")]
  public async Task It_should_delete_an_existing_region()
  {
    DeleteRegionCommand command = new(_region.Id.ToGuid());
    Region? region = await Pipeline.ExecuteAsync(command);
    Assert.NotNull(region);
    Assert.Equal(_region.Id.ToGuid(), region.Id);

    RegionEntity? entity = await PokemonContext.Regions.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == _region.Id.Value);
    Assert.Null(entity);
  }

  [Fact(DisplayName = "It should return null when the region could not be found.")]
  public async Task It_should_return_null_when_the_region_could_not_be_found()
  {
    DeleteRegionCommand command = new(Guid.NewGuid());
    Region? region = await Pipeline.ExecuteAsync(command);
    Assert.Null(region);
  }
}
