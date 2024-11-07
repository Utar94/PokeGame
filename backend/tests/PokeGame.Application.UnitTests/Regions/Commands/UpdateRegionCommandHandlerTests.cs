using FluentValidation;
using Logitar.Security.Cryptography;
using MediatR;
using Moq;
using PokeGame.Contracts;
using PokeGame.Contracts.Regions;
using PokeGame.Domain;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class UpdateRegionCommandHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IRegionQuerier> _regionQuerier = new();
  private readonly Mock<IRegionRepository> _regionRepository = new();
  private readonly Mock<ISender> _sender = new();

  private readonly UpdateRegionCommandHandler _handler;

  private readonly UserId _userId = UserId.NewId();
  private readonly Region _region;

  public UpdateRegionCommandHandlerTests()
  {
    _handler = new(_regionQuerier.Object, _regionRepository.Object, _sender.Object);

    _region = new(new UniqueName("kanto"), _userId);
    _regionRepository.Setup(x => x.LoadAsync(_region.Id, _cancellationToken)).ReturnsAsync(_region);
  }

  [Fact(DisplayName = "It should return null when the region could not be found.")]
  public async Task It_should_return_null_when_the_region_could_not_be_found()
  {
    UpdateRegionPayload payload = new();
    UpdateRegionCommand command = new(Guid.NewGuid(), payload);
    command.Contextualize();

    Assert.Null(await _handler.Handle(command, _cancellationToken));
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_is_not_valid()
  {
    UpdateRegionPayload payload = new()
    {
      UniqueName = " kanto ",
      DisplayName = new Change<string>(RandomStringGenerator.GetString(1000)),
      Link = new Change<string>("test")
    };
    UpdateRegionCommand command = new(_region.Id.ToGuid(), payload);
    command.Contextualize();

    var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command, _cancellationToken));
    Assert.Equal(3, exception.Errors.Count());
    Assert.Contains(exception.Errors, e => e.ErrorCode == "AllowedCharactersValidator" && e.PropertyName == "UniqueName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "DisplayName.Value");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "UrlValidator" && e.PropertyName == "Link.Value");
  }

  [Fact(DisplayName = "It should update an existing region.")]
  public async Task It_should_update_an_existing_region()
  {
    Description description = new("The Kanto region (Japanese: カントー地方 Kanto region) is a region of the Pokémon world. Kanto is located east of Johto, which together form a joint landmass that is south of Sinnoh.\n\nKanto is the setting of the first generation of games and can be explored in Generations II, III, IV, and VII.");
    _region.Description = description;
    _region.Update(_userId);

    UpdateRegionPayload payload = new()
    {
      UniqueName = "Kanto",
      DisplayName = new Change<string>(" Kanto "),
      Link = new Change<string>("https://bulbapedia.bulbagarden.net/wiki/Kanto")
    };
    UpdateRegionCommand command = new(_region.Id.ToGuid(), payload);
    command.Contextualize();

    RegionModel model = new();
    _regionQuerier.Setup(x => x.ReadAsync(_region, _cancellationToken)).ReturnsAsync(model);

    RegionModel? result = await _handler.Handle(command, _cancellationToken);
    Assert.NotNull(result);
    Assert.Same(model, result);

    _sender.Verify(x => x.Send(
      It.Is<SaveRegionCommand>(y => y.Region.Equals(_region) && Comparisons.AreEqual(y.Region.UniqueName, payload.UniqueName)
        && Comparisons.AreEqual(y.Region.DisplayName, payload.DisplayName.Value) && y.Region.Description == description
        && Comparisons.AreEqual(y.Region.Link, payload.Link.Value) && y.Region.Notes == null),
      _cancellationToken), Times.Once);
  }
}
