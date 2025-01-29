using Bogus;
using Logitar.EventSourcing;
using Moq;
using PokeGame.Application.Abilities.Models;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class CreateOrReplaceAbilityCommandHandlerTests
{
  private readonly ActorId _actorId = ActorId.NewId();
  private readonly CancellationToken _cancellationToken = default;
  private readonly Faker _faker = new();

  private readonly Mock<IAbilityManager> _abilityManager = new();
  private readonly Mock<IAbilityQuerier> _abilityQuerier = new();
  private readonly Mock<IAbilityRepository> _abilityRepository = new();
  private readonly Mock<IApplicationContext> _applicationContext = new();

  private readonly CreateOrReplaceAbilityCommandHandler _handler;

  public CreateOrReplaceAbilityCommandHandlerTests()
  {
    _handler = new(_abilityManager.Object, _abilityQuerier.Object, _abilityRepository.Object, _applicationContext.Object);

    _applicationContext.Setup(x => x.GetActorId()).Returns(_actorId);
  }

  [Theory(DisplayName = "It should create a new ability.")]
  [InlineData(null)]
  [InlineData("bf047435-8033-4b39-9744-362f721386c0")]
  public async Task Given_New_When_Handle_Then_Created(string? idValue)
  {
    Guid? id = idValue == null ? null : Guid.Parse(idValue);

    AbilityModel model = new();
    _abilityQuerier.Setup(x => x.ReadAsync(It.IsAny<Ability>(), _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceAbilityPayload payload = new()
    {
      UniqueName = "Overgrow",
      DisplayName = " Overgrow ",
      Description = "  Powers up Grass-type moves when the Pokémon's HP is low.  ",
      Link = "https://bulbapedia.bulbagarden.net/wiki/Overgrow_(Ability)",
      Notes = "    "
    };
    CreateOrReplaceAbilityCommand command = new(id, payload, Version: null);
    CreateOrReplaceAbilityResult result = await _handler.Handle(command, _cancellationToken);
    Assert.True(result.Created);
    Assert.NotNull(result.Ability);
    Assert.Same(model, result.Ability);

    _abilityManager.Verify(x => x.SaveAsync(
      It.Is<Ability>(y => (!id.HasValue || y.Id.ToGuid() == id.Value) && y.CreatedBy == _actorId && y.UpdatedBy == _actorId
        && Comparisons.AreEqual(payload.UniqueName, y.UniqueName)
        && Comparisons.AreEqual(payload.DisplayName, y.DisplayName)
        && Comparisons.AreEqual(payload.Description, y.Description)
        && Comparisons.AreEqual(payload.Link, y.Link)
        && Comparisons.AreEqual(payload.Notes, y.Notes)),
      _cancellationToken), Times.Once());
  }

  [Fact(DisplayName = "It should replace an existing ability.")]
  public async Task Given_Exists_When_Handle_Then_Replaced()
  {
    Ability ability = new(new UniqueName("Overgrow"));
    _abilityRepository.Setup(x => x.LoadAsync(ability.Id, _cancellationToken)).ReturnsAsync(ability);

    AbilityModel model = new();
    _abilityQuerier.Setup(x => x.ReadAsync(ability, _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceAbilityPayload payload = new()
    {
      UniqueName = "Blaze",
      DisplayName = " Blaze ",
      Description = "  Powers up Fire-type moves when the Pokémon's HP is low.  ",
      Link = "https://bulbapedia.bulbagarden.net/wiki/Blaze_(Ability)",
      Notes = "    "
    };
    CreateOrReplaceAbilityCommand command = new(ability.Id.ToGuid(), payload, Version: null);
    CreateOrReplaceAbilityResult result = await _handler.Handle(command, _cancellationToken);
    Assert.False(result.Created);
    Assert.NotNull(result.Ability);
    Assert.Same(model, result.Ability);

    _abilityManager.Verify(x => x.SaveAsync(ability, _cancellationToken), Times.Once());

    Assert.Equal(_actorId, ability.UpdatedBy);
    Assertions.Equal(payload.UniqueName, ability.UniqueName);
    Assertions.Equal(payload.DisplayName.Trim(), ability.DisplayName);
    Assertions.Equal(payload.Description, ability.Description);
    Assertions.Equal(payload.Link, ability.Link);
    Assertions.Equal(payload.Notes, ability.Notes);
  }

  [Fact(DisplayName = "It should return null when the ability was not found.")]
  public async Task Given_NotFound_When_Handle_Then_NullReturned()
  {
    CreateOrReplaceAbilityPayload payload = new()
    {
      UniqueName = "Overgrow"
    };
    CreateOrReplaceAbilityCommand command = new(Guid.NewGuid(), payload, Version: -1);
    CreateOrReplaceAbilityResult result = await _handler.Handle(command, _cancellationToken);
    Assert.Null(result.Ability);
    Assert.False(result.Created);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task Given_InvalidPayload_When_Handle_Then_ValidationException()
  {
    CreateOrReplaceAbilityPayload payload = new()
    {
      UniqueName = "Overgrow!",
      DisplayName = _faker.Random.String(999, minChar: 'A', maxChar: 'Z'),
      Link = "overgrow"
    };
    CreateOrReplaceAbilityCommand command = new(Id: null, payload, Version: null);

    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _handler.Handle(command, _cancellationToken));

    Assert.Equal(3, exception.Errors.Count());
    Assert.Contains(exception.Errors, e => e.ErrorCode == "AllowedCharactersValidator" && e.PropertyName == "UniqueName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "DisplayName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "UrlValidator" && e.PropertyName == "Link");
  }

  [Fact(DisplayName = "It should update an existing ability.")]
  public async Task Given_Exists_When_Handle_Then_Updated()
  {
    Ability ability = new(new UniqueName("Overgrow"));
    _abilityRepository.Setup(x => x.LoadAsync(ability.Id, _cancellationToken)).ReturnsAsync(ability);

    Ability reference = new(ability.UniqueName, ability.CreatedBy, ability.Id);
    _abilityRepository.Setup(x => x.LoadAsync(reference.Id, reference.Version, _cancellationToken)).ReturnsAsync(reference);

    DisplayName displayName = new("Blaze");
    ability.DisplayName = displayName;
    ability.Update();

    AbilityModel model = new();
    _abilityQuerier.Setup(x => x.ReadAsync(ability, _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceAbilityPayload payload = new()
    {
      UniqueName = "Blaze",
      DisplayName = "    ",
      Description = "  Powers up Fire-type moves when the Pokémon's HP is low.  ",
      Link = "https://bulbapedia.bulbagarden.net/wiki/Blaze_(Ability)",
      Notes = "    "
    };
    CreateOrReplaceAbilityCommand command = new(ability.Id.ToGuid(), payload, reference.Version);
    CreateOrReplaceAbilityResult result = await _handler.Handle(command, _cancellationToken);
    Assert.False(result.Created);
    Assert.NotNull(result.Ability);
    Assert.Same(model, result.Ability);

    _abilityManager.Verify(x => x.SaveAsync(ability, _cancellationToken), Times.Once());

    Assert.Equal(_actorId, ability.UpdatedBy);
    Assertions.Equal(payload.UniqueName, ability.UniqueName);
    Assert.Equal(displayName, ability.DisplayName);
    Assertions.Equal(payload.Description, ability.Description);
    Assertions.Equal(payload.Link, ability.Link);
    Assertions.Equal(payload.Notes, ability.Notes);
  }
}
