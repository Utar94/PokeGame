using Bogus;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts;
using Moq;
using PokeGame.Application.Regions.Models;
using PokeGame.Domain;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class UpdateRegionCommandHandlerTests
{
  private readonly ActorId _actorId = ActorId.NewId();
  private readonly CancellationToken _cancellationToken = default;
  private readonly Faker _faker = new();

  private readonly Mock<IApplicationContext> _applicationContext = new();
  private readonly Mock<IRegionManager> _regionManager = new();
  private readonly Mock<IRegionQuerier> _regionQuerier = new();
  private readonly Mock<IRegionRepository> _regionRepository = new();

  private readonly UpdateRegionCommandHandler _handler;

  public UpdateRegionCommandHandlerTests()
  {
    _handler = new(_applicationContext.Object, _regionManager.Object, _regionQuerier.Object, _regionRepository.Object);

    _applicationContext.Setup(x => x.GetActorId()).Returns(_actorId);
  }

  [Fact(DisplayName = "It should return null when the region was not found.")]
  public async Task Given_NotFound_When_Handle_Then_NullReturned()
  {
    UpdateRegionPayload payload = new();
    UpdateRegionCommand command = new(Guid.NewGuid(), payload);
    RegionModel? region = await _handler.Handle(command, _cancellationToken);
    Assert.Null(region);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task Given_InvalidPayload_When_Handle_Then_ValidationException()
  {
    UpdateRegionPayload payload = new()
    {
      UniqueName = "Kanto!",
      DisplayName = new ChangeModel<string>(_faker.Random.String(999, minChar: 'A', maxChar: 'Z')),
      Link = new ChangeModel<string>("kanto")
    };
    UpdateRegionCommand command = new(Guid.NewGuid(), payload);

    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _handler.Handle(command, _cancellationToken));

    Assert.Equal(3, exception.Errors.Count());
    Assert.Contains(exception.Errors, e => e.ErrorCode == "AllowedCharactersValidator" && e.PropertyName == "UniqueName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "DisplayName.Value");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "UrlValidator" && e.PropertyName == "Link.Value");
  }

  [Fact(DisplayName = "It should update an existing region.")]
  public async Task Given_Exists_When_Handle_Then_Updated()
  {
    Region region = new(new UniqueName("Kanto"));
    _regionRepository.Setup(x => x.LoadAsync(region.Id, _cancellationToken)).ReturnsAsync(region);

    DisplayName displayName = new("Johto");
    region.DisplayName = displayName;
    region.Update();

    RegionModel model = new();
    _regionQuerier.Setup(x => x.ReadAsync(region, _cancellationToken)).ReturnsAsync(model);

    UpdateRegionPayload payload = new()
    {
      UniqueName = "Johto",
      DisplayName = null,
      Description = new ChangeModel<string>("    "),
      Link = new ChangeModel<string>("https://bulbapedia.bulbagarden.net/wiki/Johto"),
      Notes = new ChangeModel<string>("  The Johto region (Japanese: ジョウト地方 Johto region) is a region of the Pokémon world. Johto is located west of Kanto, which together form a joint landmass that is south of Sinnoh and Sinjoh Ruins.  ")
    };
    UpdateRegionCommand command = new(region.Id.ToGuid(), payload);
    RegionModel? result = await _handler.Handle(command, _cancellationToken);
    Assert.NotNull(result);
    Assert.Same(model, result);

    _regionManager.Verify(x => x.SaveAsync(region, _cancellationToken), Times.Once());

    Assert.Equal(_actorId, region.UpdatedBy);
    Assertions.Equal(payload.UniqueName, region.UniqueName);
    Assert.Equal(displayName, region.DisplayName);
    Assertions.Equal(payload.Description.Value, region.Description);
    Assertions.Equal(payload.Link.Value, region.Link);
    Assertions.Equal(payload.Notes.Value, region.Notes);
  }
}
