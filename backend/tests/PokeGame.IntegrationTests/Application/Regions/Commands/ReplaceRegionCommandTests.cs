using FluentValidation.Results;
using Logitar;
using Logitar.Identity.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts.Regions;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

[Trait(Traits.Category, Categories.Integration)]
public class ReplaceRegionCommandTests : IntegrationTests
{
  private readonly IRegionRepository _regionRepository;

  private readonly RegionAggregate _region;

  public ReplaceRegionCommandTests() : base()
  {
    _regionRepository = ServiceProvider.GetRequiredService<IRegionRepository>();

    _region = new(new UniqueNameUnit(RegionAggregate.UniqueNameSettings, "Kanto"), ActorId);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _regionRepository.SaveAsync(_region);
  }

  [Fact(DisplayName = "It should replace an existing region.")]
  public async Task It_should_replace_an_existing_region()
  {
    long version = _region.Version;

    _region.Description = new DescriptionUnit("Kanto is the setting of the first generation of games and can be explored in Generations II, III, IV, and VII.");
    _region.Update(ActorId);
    await _regionRepository.SaveAsync(_region);

    ReplaceRegionPayload payload = new("Kanto")
    {
      DisplayName = "  Kanto  ",
      Description = "    ",
      Reference = "https://bulbapedia.bulbagarden.net/wiki/Kanto",
      Notes = "    "
    };
    ReplaceRegionCommand command = new(_region.Id.ToGuid(), payload, version);
    Region? region = await Pipeline.ExecuteAsync(command);
    Assert.NotNull(region);

    Assert.Equal(_region.Id.ToGuid(), region.Id);
    Assert.Equal(_region.Version + 1, region.Version);
    Assert.Equal(Actor, region.CreatedBy);
    Assert.Equal(Actor, region.UpdatedBy);
    Assert.True(region.CreatedOn < region.UpdatedOn);

    Assert.Equal(payload.UniqueName, region.UniqueName);
    Assert.Equal(payload.DisplayName.CleanTrim(), region.DisplayName);
    Assert.Equal(_region.Description.Value, region.Description);
    Assert.Equal(payload.Reference.CleanTrim(), region.Reference);
    Assert.Equal(payload.Notes.CleanTrim(), region.Notes);
  }

  [Fact(DisplayName = "It should return null when the region could not be found.")]
  public async Task It_should_return_null_when_the_region_could_not_be_found()
  {
    ReplaceRegionPayload payload = new("Kanto");
    ReplaceRegionCommand command = new(Id: Guid.NewGuid(), payload, Version: null);
    Assert.Null(await Pipeline.ExecuteAsync(command));
  }

  [Fact(DisplayName = "It should throw UniqueNameAlreadyUsedException when the unique name is already used.")]
  public async Task It_should_throw_UniqueNameAlreadyUsedException_when_the_unique_name_is_already_used()
  {
    RegionAggregate region = new(new UniqueNameUnit(RegionAggregate.UniqueNameSettings, "Johto"), ActorId);
    await _regionRepository.SaveAsync(region);

    ReplaceRegionPayload payload = new(region.UniqueName.Value);
    ReplaceRegionCommand command = new(_region.Id.ToGuid(), payload, Version: null);
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<RegionAggregate>>(async () => await Pipeline.ExecuteAsync(command));
    Assert.Equal(payload.UniqueName, exception.UniqueName);
    Assert.Equal(nameof(payload.UniqueName), exception.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_is_not_valid()
  {
    ReplaceRegionPayload payload = new("Kanto!");
    ReplaceRegionCommand command = new(_region.Id.ToGuid(), payload, Version: null);
    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await Pipeline.ExecuteAsync(command));
    ValidationFailure error = Assert.Single(exception.Errors);
    Assert.Equal(nameof(AllowedCharactersValidator), error.ErrorCode);
    Assert.Equal("UniqueName", error.PropertyName);
  }
}
