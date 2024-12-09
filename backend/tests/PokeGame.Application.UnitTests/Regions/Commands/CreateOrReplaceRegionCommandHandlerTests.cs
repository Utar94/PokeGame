﻿using FluentValidation;
using Logitar.Security.Cryptography;
using MediatR;
using Moq;
using PokeGame.Contracts.Regions;
using PokeGame.Domain;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class CreateOrReplaceRegionCommandHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IRegionQuerier> _regionQuerier = new();
  private readonly Mock<IRegionRepository> _regionRepository = new();
  private readonly Mock<ISender> _sender = new();

  private readonly CreateOrReplaceRegionCommandHandler _handler;

  private readonly UserId _userId = UserId.NewId();
  private readonly Region _region;

  public CreateOrReplaceRegionCommandHandlerTests()
  {
    _handler = new(_regionQuerier.Object, _regionRepository.Object, _sender.Object);

    _region = new(new UniqueName("kanto"), _userId);
    _regionRepository.Setup(x => x.LoadAsync(_region.Id, _cancellationToken)).ReturnsAsync(_region);
  }

  [Fact(DisplayName = "It should create a new region.")]
  public async Task It_should_create_a_new_region()
  {
    CreateOrReplaceRegionPayload payload = new("Kanto")
    {
      DisplayName = " Kanto ",
      Description = "  The Kanto region (Japanese: カントー地方 Kanto region) is a region of the Pokémon world. Kanto is located east of Johto, which together form a joint landmass that is south of Sinnoh.\n\nKanto is the setting of the first generation of games and can be explored in Generations II, III, IV, and VII.  ",
      Link = "https://bulbapedia.bulbagarden.net/wiki/Kanto",
      Notes = "    "
    };
    CreateOrReplaceRegionCommand command = new(Id: Guid.NewGuid(), payload, Version: null);
    command.Contextualize();

    RegionModel model = new();
    _regionQuerier.Setup(x => x.ReadAsync(It.IsAny<Region>(), _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceRegionResult result = await _handler.Handle(command, _cancellationToken);
    Assert.Same(model, result.Region);
    Assert.True(result.Created);

    _sender.Verify(x => x.Send(
      It.Is<SaveRegionCommand>(y => y.Region.Id.ToGuid() == command.Id && Comparisons.AreEqual(y.Region.UniqueName, payload.UniqueName)
        && Comparisons.AreEqual(y.Region.DisplayName, payload.DisplayName) && Comparisons.AreEqual(y.Region.Description, payload.Description)
        && Comparisons.AreEqual(y.Region.Link, payload.Link) && Comparisons.AreEqual(y.Region.Notes, payload.Notes)),
      _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should replace an existing region.")]
  public async Task It_should_replace_an_existing_region()
  {
    CreateOrReplaceRegionPayload payload = new("Kanto")
    {
      DisplayName = " Kanto ",
      Description = "  The Kanto region (Japanese: カントー地方 Kanto region) is a region of the Pokémon world. Kanto is located east of Johto, which together form a joint landmass that is south of Sinnoh.\n\nKanto is the setting of the first generation of games and can be explored in Generations II, III, IV, and VII.  ",
      Link = "https://bulbapedia.bulbagarden.net/wiki/Kanto",
      Notes = "    "
    };
    CreateOrReplaceRegionCommand command = new(_region.Id.ToGuid(), payload, Version: null);
    command.Contextualize();

    RegionModel model = new();
    _regionQuerier.Setup(x => x.ReadAsync(_region, _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceRegionResult result = await _handler.Handle(command, _cancellationToken);
    Assert.Same(model, result.Region);
    Assert.False(result.Created);

    _sender.Verify(x => x.Send(
      It.Is<SaveRegionCommand>(y => y.Region.Equals(_region) && Comparisons.AreEqual(y.Region.UniqueName, payload.UniqueName)
        && Comparisons.AreEqual(y.Region.DisplayName, payload.DisplayName) && Comparisons.AreEqual(y.Region.Description, payload.Description)
        && Comparisons.AreEqual(y.Region.Link, payload.Link) && Comparisons.AreEqual(y.Region.Notes, payload.Notes)),
      _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should return empty when updating an region that does not exist.")]
  public async Task It_should_return_empty_when_updating_an_region_that_does_not_exist()
  {
    CreateOrReplaceRegionPayload payload = new("Kanto");
    CreateOrReplaceRegionCommand command = new(Guid.NewGuid(), payload, Version: 1);
    command.Contextualize();

    CreateOrReplaceRegionResult result = await _handler.Handle(command, _cancellationToken);
    Assert.Null(result.Region);
    Assert.False(result.Created);

    _regionRepository.Verify(x => x.SaveAsync(It.IsAny<Region>(), _cancellationToken), Times.Never);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_is_not_valid()
  {
    CreateOrReplaceRegionPayload payload = new(" Kanto ")
    {
      DisplayName = RandomStringGenerator.GetString(1000),
      Link = "test"
    };
    CreateOrReplaceRegionCommand command = new(Id: null, payload, Version: null);
    command.Contextualize();

    var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command, _cancellationToken));
    Assert.Equal(3, exception.Errors.Count());
    Assert.Contains(exception.Errors, e => e.ErrorCode == "AllowedCharactersValidator" && e.PropertyName == "UniqueName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "DisplayName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "UrlValidator" && e.PropertyName == "Link");
  }

  [Fact(DisplayName = "It should update an existing region.")]
  public async Task It_should_update_an_existing_region()
  {
    Region reference = new(_region.UniqueName, _userId, _region.Id);
    _regionRepository.Setup(x => x.LoadAsync(reference.Id, reference.Version, _cancellationToken)).ReturnsAsync(reference);

    Description description = new("The Kanto region (Japanese: カントー地方 Kanto region) is a region of the Pokémon world. Kanto is located east of Johto, which together form a joint landmass that is south of Sinnoh.\n\nKanto is the setting of the first generation of games and can be explored in Generations II, III, IV, and VII.");
    _region.Description = description;
    _region.Update(_userId);

    CreateOrReplaceRegionPayload payload = new("Kanto")
    {
      DisplayName = " Kanto ",
      Description = "    ",
      Link = "https://bulbapedia.bulbagarden.net/wiki/Kanto",
      Notes = "    "
    };
    CreateOrReplaceRegionCommand command = new(_region.Id.ToGuid(), payload, reference.Version);
    command.Contextualize();

    RegionModel model = new();
    _regionQuerier.Setup(x => x.ReadAsync(_region, _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceRegionResult result = await _handler.Handle(command, _cancellationToken);
    Assert.Same(model, result.Region);
    Assert.False(result.Created);

    _sender.Verify(x => x.Send(
      It.Is<SaveRegionCommand>(y => y.Region.Equals(_region) && Comparisons.AreEqual(y.Region.UniqueName, payload.UniqueName)
        && Comparisons.AreEqual(y.Region.DisplayName, payload.DisplayName) && y.Region.Description == description
        && Comparisons.AreEqual(y.Region.Link, payload.Link) && Comparisons.AreEqual(y.Region.Notes, payload.Notes)),
      _cancellationToken), Times.Once);
  }
}
