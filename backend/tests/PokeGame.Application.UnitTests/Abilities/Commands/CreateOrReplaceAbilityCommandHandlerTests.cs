﻿using FluentValidation;
using Logitar.Security.Cryptography;
using MediatR;
using Moq;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class CreateOrReplaceAbilityCommandHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IAbilityQuerier> _abilityQuerier = new();
  private readonly Mock<IAbilityRepository> _abilityRepository = new();
  private readonly Mock<ISender> _sender = new();

  private readonly CreateOrReplaceAbilityCommandHandler _handler;

  private readonly UserId _userId = UserId.NewId();
  private readonly Ability _ability;

  public CreateOrReplaceAbilityCommandHandlerTests()
  {
    _handler = new(_abilityQuerier.Object, _abilityRepository.Object, _sender.Object);

    _ability = new(new UniqueName("adaptability"), _userId);
    _abilityRepository.Setup(x => x.LoadAsync(_ability.Id, _cancellationToken)).ReturnsAsync(_ability);
  }

  [Fact(DisplayName = "It should create a new ability.")]
  public async Task It_should_create_a_new_ability()
  {
    CreateOrReplaceAbilityPayload payload = new("Adaptability")
    {
      DisplayName = " Adaptability ",
      Description = "  Powers up moves of the same type as the Pokémon.  ",
      Link = "https://bulbapedia.bulbagarden.net/wiki/Adaptability_(Ability)",
      Notes = "    "
    };
    CreateOrReplaceAbilityCommand command = new(Id: Guid.NewGuid(), payload, Version: null);
    command.Contextualize();

    AbilityModel model = new();
    _abilityQuerier.Setup(x => x.ReadAsync(It.IsAny<Ability>(), _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceAbilityResult result = await _handler.Handle(command, _cancellationToken);
    Assert.Same(model, result.Ability);
    Assert.True(result.Created);

    _sender.Verify(x => x.Send(
      It.Is<SaveAbilityCommand>(y => y.Ability.Id.ToGuid() == command.Id && Comparisons.AreEqual(y.Ability.UniqueName, payload.UniqueName)
        && Comparisons.AreEqual(y.Ability.DisplayName, payload.DisplayName) && Comparisons.AreEqual(y.Ability.Description, payload.Description)
        && Comparisons.AreEqual(y.Ability.Link, payload.Link) && Comparisons.AreEqual(y.Ability.Notes, payload.Notes)),
      _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should replace an existing ability.")]
  public async Task It_should_replace_an_existing_ability()
  {
    CreateOrReplaceAbilityPayload payload = new("Adaptability")
    {
      DisplayName = " Adaptability ",
      Description = "  Powers up moves of the same type as the Pokémon.  ",
      Link = "https://bulbapedia.bulbagarden.net/wiki/Adaptability_(Ability)",
      Notes = "    "
    };
    CreateOrReplaceAbilityCommand command = new(_ability.Id.ToGuid(), payload, Version: null);
    command.Contextualize();

    AbilityModel model = new();
    _abilityQuerier.Setup(x => x.ReadAsync(_ability, _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceAbilityResult result = await _handler.Handle(command, _cancellationToken);
    Assert.Same(model, result.Ability);
    Assert.False(result.Created);

    _sender.Verify(x => x.Send(
      It.Is<SaveAbilityCommand>(y => y.Ability.Equals(_ability) && Comparisons.AreEqual(y.Ability.UniqueName, payload.UniqueName)
        && Comparisons.AreEqual(y.Ability.DisplayName, payload.DisplayName) && Comparisons.AreEqual(y.Ability.Description, payload.Description)
        && Comparisons.AreEqual(y.Ability.Link, payload.Link) && Comparisons.AreEqual(y.Ability.Notes, payload.Notes)),
      _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should return empty when updating an ability that does not exist.")]
  public async Task It_should_return_empty_when_updating_an_ability_that_does_not_exist()
  {
    CreateOrReplaceAbilityPayload payload = new("Adaptability")
    {
      DisplayName = " Adaptability ",
      Description = "  Powers up moves of the same type as the Pokémon.  ",
      Link = "https://bulbapedia.bulbagarden.net/wiki/Adaptability_(Ability)",
      Notes = "    "
    };
    CreateOrReplaceAbilityCommand command = new(Guid.NewGuid(), payload, Version: 1);
    command.Contextualize();

    CreateOrReplaceAbilityResult result = await _handler.Handle(command, _cancellationToken);
    Assert.Null(result.Ability);
    Assert.False(result.Created);

    _abilityRepository.Verify(x => x.SaveAsync(It.IsAny<Ability>(), _cancellationToken), Times.Never);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_is_not_valid()
  {
    CreateOrReplaceAbilityPayload payload = new(" Adaptability ")
    {
      DisplayName = RandomStringGenerator.GetString(1000),
      Link = "test"
    };
    CreateOrReplaceAbilityCommand command = new(Id: null, payload, Version: null);
    command.Contextualize();

    var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command, _cancellationToken));
    Assert.Equal(3, exception.Errors.Count());
    Assert.Contains(exception.Errors, e => e.ErrorCode == "AllowedCharactersValidator" && e.PropertyName == "UniqueName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "DisplayName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "UrlValidator" && e.PropertyName == "Link");
  }

  [Fact(DisplayName = "It should update an existing ability.")]
  public async Task It_should_update_an_existing_ability()
  {
    Ability reference = new(_ability.UniqueName, _userId, _ability.Id);
    _abilityRepository.Setup(x => x.LoadAsync(reference.Id, reference.Version, _cancellationToken)).ReturnsAsync(reference);

    Notes notes = new("Adaptability increases STAB of a Pokémon with this Ability from 1.5 to 2.");
    _ability.Notes = notes;
    _ability.Update(_userId);

    CreateOrReplaceAbilityPayload payload = new("Adaptability")
    {
      DisplayName = " Adaptability ",
      Description = "  Powers up moves of the same type as the Pokémon.  ",
      Link = "https://bulbapedia.bulbagarden.net/wiki/Adaptability_(Ability)",
      Notes = "    "
    };
    CreateOrReplaceAbilityCommand command = new(_ability.Id.ToGuid(), payload, reference.Version);
    command.Contextualize();

    AbilityModel model = new();
    _abilityQuerier.Setup(x => x.ReadAsync(_ability, _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceAbilityResult result = await _handler.Handle(command, _cancellationToken);
    Assert.Same(model, result.Ability);
    Assert.False(result.Created);

    _sender.Verify(x => x.Send(
      It.Is<SaveAbilityCommand>(y => y.Ability.Equals(_ability) && Comparisons.AreEqual(y.Ability.UniqueName, payload.UniqueName)
        && Comparisons.AreEqual(y.Ability.DisplayName, payload.DisplayName) && Comparisons.AreEqual(y.Ability.Description, payload.Description)
        && Comparisons.AreEqual(y.Ability.Link, payload.Link) && y.Ability.Notes == notes),
      _cancellationToken), Times.Once);
  }
}
