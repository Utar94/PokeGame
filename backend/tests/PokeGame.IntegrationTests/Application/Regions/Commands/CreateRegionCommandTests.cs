using FluentValidation.Results;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts.Regions;
using PokeGame.Domain.Regions;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.Application.Regions.Commands;

[Trait(Traits.Category, Categories.Integration)]
public class CreateRegionCommandTests : IntegrationTests
{
  private readonly IRegionRepository _regionRepository;

  public CreateRegionCommandTests() : base()
  {
    _regionRepository = ServiceProvider.GetRequiredService<IRegionRepository>();
  }

  [Fact(DisplayName = "It should create a new region.")]
  public async Task It_should_create_a_new_region()
  {
    CreateRegionPayload payload = new("Kanto");
    CreateRegionCommand command = new(payload);
    Region region = await Pipeline.ExecuteAsync(command);

    Assert.NotEqual(Guid.Empty, region.Id);
    Assert.Equal(1, region.Version);
    Assert.Equal(Actor, region.CreatedBy);
    Assert.Equal(Actor, region.UpdatedBy);
    Assert.Equal(region.CreatedOn, region.UpdatedOn);

    Assert.Equal(payload.UniqueName, region.UniqueName);

    RegionEntity? entity = await PokemonContext.Regions.AsNoTracking().SingleOrDefaultAsync(x => x.AggregateId == new AggregateId(region.Id).Value);
    Assert.NotNull(entity);
  }

  [Fact(DisplayName = "It should throw UniqueNameAlreadyUsedException when the unique name is already used.")]
  public async Task It_should_throw_UniqueNameAlreadyUsedException_when_the_unique_name_is_already_used()
  {
    RegionAggregate region = new(new UniqueNameUnit(RegionAggregate.UniqueNameSettings, "Kanto"));
    await _regionRepository.SaveAsync(region);

    CreateRegionPayload payload = new(region.UniqueName.Value);
    CreateRegionCommand command = new(payload);
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<RegionAggregate>>(async () => await Pipeline.ExecuteAsync(command));
    Assert.Equal(payload.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_is_not_valid()
  {
    CreateRegionPayload payload = new("Kanto!");
    CreateRegionCommand command = new(payload);
    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await Pipeline.ExecuteAsync(command));
    ValidationFailure error = Assert.Single(exception.Errors);
    Assert.Equal(nameof(AllowedCharactersValidator), error.ErrorCode);
    Assert.Equal("UniqueName", error.PropertyName);
  }
}
