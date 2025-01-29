using Bogus;
using Logitar.EventSourcing;
using Moq;
using PokeGame.Application.Regions.Models;
using PokeGame.Domain;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class CreateOrReplaceRegionCommandHandlerTests
{
  private readonly ActorId _actorId = ActorId.NewId();
  private readonly CancellationToken _cancellationToken = default;
  private readonly Faker _faker = new();

  private readonly Mock<IApplicationContext> _applicationContext = new();
  private readonly Mock<IRegionManager> _regionManager = new();
  private readonly Mock<IRegionQuerier> _regionQuerier = new();
  private readonly Mock<IRegionRepository> _regionRepository = new();

  private readonly CreateOrReplaceRegionCommandHandler _handler;

  public CreateOrReplaceRegionCommandHandlerTests()
  {
    _handler = new(_applicationContext.Object, _regionManager.Object, _regionQuerier.Object, _regionRepository.Object);

    _applicationContext.Setup(x => x.GetActorId()).Returns(_actorId);
  }

  [Theory(DisplayName = "It should create a new region.")]
  [InlineData(null)]
  [InlineData("ceac7d3d-e332-422b-bb4e-8c5d9d3fafd2")]
  public async Task Given_New_When_Handle_Then_Created(string? idValue)
  {
    Guid? id = idValue == null ? null : Guid.Parse(idValue);

    RegionModel model = new();
    _regionQuerier.Setup(x => x.ReadAsync(It.IsAny<Region>(), _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceRegionPayload payload = new()
    {
      UniqueName = "Kanto",
      DisplayName = " Kanto ",
      Description = "    ",
      Link = "https://bulbapedia.bulbagarden.net/wiki/Kanto",
      Notes = "  The Kanto region (Japanese: カントー地方 Kanto region) is a region of the Pokémon world. Kanto is located east of Johto, which together form a joint landmass that is south of Sinnoh.  "
    };
    CreateOrReplaceRegionCommand command = new(id, payload, Version: null);
    CreateOrReplaceRegionResult result = await _handler.Handle(command, _cancellationToken);
    Assert.True(result.Created);
    Assert.NotNull(result.Region);
    Assert.Same(model, result.Region);

    _regionManager.Verify(x => x.SaveAsync(
      It.Is<Region>(y => (!id.HasValue || y.Id.ToGuid() == id.Value) && y.CreatedBy == _actorId && y.UpdatedBy == _actorId
        && Comparisons.AreEqual(payload.UniqueName, y.UniqueName)
        && Comparisons.AreEqual(payload.DisplayName, y.DisplayName)
        && Comparisons.AreEqual(payload.Description, y.Description)
        && Comparisons.AreEqual(payload.Link, y.Link)
        && Comparisons.AreEqual(payload.Notes, y.Notes)),
      _cancellationToken), Times.Once());
  }

  [Fact(DisplayName = "It should replace an existing region.")]
  public async Task Given_Exists_When_Handle_Then_Replaced()
  {
    Region region = new(new UniqueName("Kanto"));
    _regionRepository.Setup(x => x.LoadAsync(region.Id, _cancellationToken)).ReturnsAsync(region);

    RegionModel model = new();
    _regionQuerier.Setup(x => x.ReadAsync(region, _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceRegionPayload payload = new()
    {
      UniqueName = "Johto",
      DisplayName = " Johto ",
      Description = "    ",
      Link = "https://bulbapedia.bulbagarden.net/wiki/Johto",
      Notes = "  The Johto region (Japanese: ジョウト地方 Johto region) is a region of the Pokémon world. Johto is located west of Kanto, which together form a joint landmass that is south of Sinnoh and Sinjoh Ruins.  "
    };
    CreateOrReplaceRegionCommand command = new(region.Id.ToGuid(), payload, Version: null);
    CreateOrReplaceRegionResult result = await _handler.Handle(command, _cancellationToken);
    Assert.False(result.Created);
    Assert.NotNull(result.Region);
    Assert.Same(model, result.Region);

    _regionManager.Verify(x => x.SaveAsync(region, _cancellationToken), Times.Once());

    Assert.Equal(_actorId, region.UpdatedBy);
    Assertions.Equal(payload.UniqueName, region.UniqueName);
    Assertions.Equal(payload.DisplayName.Trim(), region.DisplayName);
    Assertions.Equal(payload.Description, region.Description);
    Assertions.Equal(payload.Link, region.Link);
    Assertions.Equal(payload.Notes, region.Notes);
  }

  [Fact(DisplayName = "It should return null when the region was not found.")]
  public async Task Given_NotFound_When_Handle_Then_NullReturned()
  {
    CreateOrReplaceRegionPayload payload = new()
    {
      UniqueName = "Kanto"
    };
    CreateOrReplaceRegionCommand command = new(Guid.NewGuid(), payload, Version: -1);
    CreateOrReplaceRegionResult result = await _handler.Handle(command, _cancellationToken);
    Assert.Null(result.Region);
    Assert.False(result.Created);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task Given_InvalidPayload_When_Handle_Then_ValidationException()
  {
    CreateOrReplaceRegionPayload payload = new()
    {
      UniqueName = "Kanto!",
      DisplayName = _faker.Random.String(999, minChar: 'A', maxChar: 'Z'),
      Link = "kanto"
    };
    CreateOrReplaceRegionCommand command = new(Id: null, payload, Version: null);

    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _handler.Handle(command, _cancellationToken));

    Assert.Equal(3, exception.Errors.Count());
    Assert.Contains(exception.Errors, e => e.ErrorCode == "AllowedCharactersValidator" && e.PropertyName == "UniqueName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "DisplayName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "UrlValidator" && e.PropertyName == "Link");
  }

  [Fact(DisplayName = "It should update an existing region.")]
  public async Task Given_Exists_When_Handle_Then_Updated()
  {
    Region region = new(new UniqueName("Kanto"));
    _regionRepository.Setup(x => x.LoadAsync(region.Id, _cancellationToken)).ReturnsAsync(region);

    Region reference = new(region.UniqueName, region.CreatedBy, region.Id);
    _regionRepository.Setup(x => x.LoadAsync(reference.Id, reference.Version, _cancellationToken)).ReturnsAsync(reference);

    DisplayName displayName = new("Johto");
    region.DisplayName = displayName;
    region.Update();

    RegionModel model = new();
    _regionQuerier.Setup(x => x.ReadAsync(region, _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceRegionPayload payload = new()
    {
      UniqueName = "Johto",
      DisplayName = "    ",
      Description = "    ",
      Link = "https://bulbapedia.bulbagarden.net/wiki/Johto",
      Notes = "  The Johto region (Japanese: ジョウト地方 Johto region) is a region of the Pokémon world. Johto is located west of Kanto, which together form a joint landmass that is south of Sinnoh and Sinjoh Ruins.  "
    };
    CreateOrReplaceRegionCommand command = new(region.Id.ToGuid(), payload, reference.Version);
    CreateOrReplaceRegionResult result = await _handler.Handle(command, _cancellationToken);
    Assert.False(result.Created);
    Assert.NotNull(result.Region);
    Assert.Same(model, result.Region);

    _regionManager.Verify(x => x.SaveAsync(region, _cancellationToken), Times.Once());

    Assert.Equal(_actorId, region.UpdatedBy);
    Assertions.Equal(payload.UniqueName, region.UniqueName);
    Assert.Equal(displayName, region.DisplayName);
    Assertions.Equal(payload.Description, region.Description);
    Assertions.Equal(payload.Link, region.Link);
    Assertions.Equal(payload.Notes, region.Notes);
  }
}
