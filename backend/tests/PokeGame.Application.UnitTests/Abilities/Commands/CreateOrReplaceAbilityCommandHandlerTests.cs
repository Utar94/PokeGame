using FluentValidation;
using Logitar.EventSourcing;
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

  private readonly CreateOrReplaceAbilityCommandHandler _handler;

  private readonly Ability _ability = new(new Name("Adaptability"), new ActorId());

  public CreateOrReplaceAbilityCommandHandlerTests()
  {
    _handler = new(_abilityQuerier.Object, _abilityRepository.Object);

    _abilityRepository.Setup(x => x.LoadAsync(_ability.Id, _cancellationToken)).ReturnsAsync(_ability);
  }

  [Fact(DisplayName = "It should create a new ability.")]
  public async Task It_should_create_a_new_ability()
  {
    CreateOrReplaceAbilityPayload payload = new(" Adaptability ")
    {
      Kind = AbilityKind.Adaptability,
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

    _abilityRepository.Verify(x => x.SaveAsync(
      It.Is<Ability>(y => y.Kind == payload.Kind
        && Comparisons.AreEqual(y.Name, payload.Name) && Comparisons.AreEqual(y.Description, payload.Description)
        && Comparisons.AreEqual(y.Link, payload.Link) && Comparisons.AreEqual(y.Notes, payload.Notes)),
      _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should replace an existing ability.")]
  public async Task It_should_replace_an_existing_ability()
  {
    CreateOrReplaceAbilityPayload payload = new(" Adaptability ")
    {
      Kind = AbilityKind.Adaptability,
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

    _abilityRepository.Verify(x => x.SaveAsync(
      It.Is<Ability>(y => y.Kind == payload.Kind
        && Comparisons.AreEqual(y.Name, payload.Name) && Comparisons.AreEqual(y.Description, payload.Description)
        && Comparisons.AreEqual(y.Link, payload.Link) && Comparisons.AreEqual(y.Notes, payload.Notes)),
      _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should return empty when updating an ability that does not exist.")]
  public async Task It_should_return_empty_when_updating_an_ability_that_does_not_exist()
  {
    CreateOrReplaceAbilityPayload payload = new(" Adaptability ")
    {
      Kind = AbilityKind.Adaptability,
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
    CreateOrReplaceAbilityPayload payload = new("Adaptability")
    {
      Kind = (AbilityKind)(-1),
      Link = "test"
    };
    CreateOrReplaceAbilityCommand command = new(Id: null, payload, Version: null);
    command.Contextualize();

    var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command, _cancellationToken));
    Assert.Equal(2, exception.Errors.Count());
    Assert.Contains(exception.Errors, e => e.ErrorCode == "EnumValidator" && e.PropertyName == "Kind");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "UrlValidator" && e.PropertyName == "Link");
  }

  [Fact(DisplayName = "It should update an existing ability.")]
  public async Task It_should_update_an_existing_ability()
  {
    Ability reference = new(_ability.Name, _ability.CreatedBy, _ability.Id);
    _abilityRepository.Setup(x => x.LoadAsync(reference.Id, reference.Version, _cancellationToken)).ReturnsAsync(reference);

    Notes notes = new("Adaptability increases STAB of a Pokémon with this Ability from 1.5 to 2.");
    _ability.Notes = notes;
    _ability.Update(_ability.CreatedBy);

    CreateOrReplaceAbilityPayload payload = new(" Adaptability ")
    {
      Kind = AbilityKind.Adaptability,
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

    _abilityRepository.Verify(x => x.SaveAsync(
      It.Is<Ability>(y => y.Kind == payload.Kind
        && Comparisons.AreEqual(y.Name, payload.Name) && Comparisons.AreEqual(y.Description, payload.Description)
        && Comparisons.AreEqual(y.Link, payload.Link) && y.Notes == notes),
      _cancellationToken), Times.Once);
  }
}
