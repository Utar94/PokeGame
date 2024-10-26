using FluentValidation;
using Moq;
using PokeGame.Contracts;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class UpdateAbilityCommandHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IAbilityQuerier> _abilityQuerier = new();
  private readonly Mock<IAbilityRepository> _abilityRepository = new();

  private readonly UpdateAbilityCommandHandler _handler;

  private readonly UserId _userId = UserId.NewId();
  private readonly Ability _ability;

  public UpdateAbilityCommandHandlerTests()
  {
    _handler = new(_abilityQuerier.Object, _abilityRepository.Object);

    _ability = new(new Name("Adaptability"), _userId);
    _abilityRepository.Setup(x => x.LoadAsync(_ability.Id, _cancellationToken)).ReturnsAsync(_ability);
  }

  [Fact(DisplayName = "It should return null when the ability could not be found.")]
  public async Task It_should_return_null_when_the_ability_could_not_be_found()
  {
    UpdateAbilityPayload payload = new();
    UpdateAbilityCommand command = new(Guid.NewGuid(), payload);
    command.Contextualize();

    Assert.Null(await _handler.Handle(command, _cancellationToken));
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_is_not_valid()
  {
    UpdateAbilityPayload payload = new()
    {
      Kind = new Change<AbilityKind?>((AbilityKind)(-1)),
      Link = new Change<string>("test")
    };
    UpdateAbilityCommand command = new(_ability.Id.ToGuid(), payload);
    command.Contextualize();

    var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command, _cancellationToken));
    Assert.Equal(2, exception.Errors.Count());
    Assert.Contains(exception.Errors, e => e.ErrorCode == "EnumValidator" && e.PropertyName == "Kind.Value");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "UrlValidator" && e.PropertyName == "Link.Value");
  }

  [Fact(DisplayName = "It should update an existing ability.")]
  public async Task It_should_update_an_existing_ability()
  {
    Notes notes = new("Adaptability increases STAB of a Pokémon with this Ability from 1.5 to 2.");
    _ability.Notes = notes;
    _ability.Update(_userId);

    UpdateAbilityPayload payload = new()
    {
      Name = " Adaptability ",
      Kind = new Change<AbilityKind?>(AbilityKind.Adaptability),
      Description = new Change<string>("  Powers up moves of the same type as the Pokémon.  "),
      Link = new Change<string>("https://bulbapedia.bulbagarden.net/wiki/Adaptability_(Ability)")
    };
    UpdateAbilityCommand command = new(_ability.Id.ToGuid(), payload);
    command.Contextualize();

    AbilityModel model = new();
    _abilityQuerier.Setup(x => x.ReadAsync(_ability, _cancellationToken)).ReturnsAsync(model);

    AbilityModel? result = await _handler.Handle(command, _cancellationToken);
    Assert.NotNull(result);
    Assert.Same(model, result);

    _abilityRepository.Verify(x => x.SaveAsync(
      It.Is<Ability>(y => y.Kind == payload.Kind.Value
        && Comparisons.AreEqual(y.Name, payload.Name) && Comparisons.AreEqual(y.Description, payload.Description.Value)
        && Comparisons.AreEqual(y.Link, payload.Link.Value) && y.Notes == notes),
      _cancellationToken), Times.Once);
  }
}
