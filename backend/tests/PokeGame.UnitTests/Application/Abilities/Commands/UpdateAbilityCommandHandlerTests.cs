using Bogus;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts;
using Moq;
using PokeGame.Application.Abilities.Models;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class UpdateAbilityCommandHandlerTests
{
  private readonly ActorId _actorId = ActorId.NewId();
  private readonly CancellationToken _cancellationToken = default;
  private readonly Faker _faker = new();

  private readonly Mock<IApplicationContext> _applicationContext = new();
  private readonly Mock<IAbilityManager> _abilityManager = new();
  private readonly Mock<IAbilityQuerier> _abilityQuerier = new();
  private readonly Mock<IAbilityRepository> _abilityRepository = new();

  private readonly UpdateAbilityCommandHandler _handler;

  public UpdateAbilityCommandHandlerTests()
  {
    _handler = new(_abilityManager.Object, _abilityQuerier.Object, _abilityRepository.Object, _applicationContext.Object);

    _applicationContext.Setup(x => x.GetActorId()).Returns(_actorId);
  }

  [Fact(DisplayName = "It should return null when the ability was not found.")]
  public async Task Given_NotFound_When_Handle_Then_NullReturned()
  {
    UpdateAbilityPayload payload = new();
    UpdateAbilityCommand command = new(Guid.NewGuid(), payload);
    AbilityModel? ability = await _handler.Handle(command, _cancellationToken);
    Assert.Null(ability);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task Given_InvalidPayload_When_Handle_Then_ValidationException()
  {
    UpdateAbilityPayload payload = new()
    {
      UniqueName = "Overgrow!",
      DisplayName = new ChangeModel<string>(_faker.Random.String(999, minChar: 'A', maxChar: 'Z')),
      Link = new ChangeModel<string>("overgrow")
    };
    UpdateAbilityCommand command = new(Guid.NewGuid(), payload);

    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _handler.Handle(command, _cancellationToken));

    Assert.Equal(3, exception.Errors.Count());
    Assert.Contains(exception.Errors, e => e.ErrorCode == "AllowedCharactersValidator" && e.PropertyName == "UniqueName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "DisplayName.Value");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "UrlValidator" && e.PropertyName == "Link.Value");
  }

  [Fact(DisplayName = "It should update an existing ability.")]
  public async Task Given_Exists_When_Handle_Then_Updated()
  {
    Ability ability = new(new UniqueName("Overgrow"));
    _abilityRepository.Setup(x => x.LoadAsync(ability.Id, _cancellationToken)).ReturnsAsync(ability);

    DisplayName displayName = new("Blaze");
    ability.DisplayName = displayName;
    ability.Update();

    AbilityModel model = new();
    _abilityQuerier.Setup(x => x.ReadAsync(ability, _cancellationToken)).ReturnsAsync(model);

    UpdateAbilityPayload payload = new()
    {
      UniqueName = "Blaze",
      DisplayName = null,
      Description = new ChangeModel<string>("  Powers up Fire-type moves when the Pokémon's HP is low.  "),
      Link = new ChangeModel<string>("https://bulbapedia.bulbagarden.net/wiki/Blaze_(Ability)"),
      Notes = new ChangeModel<string>("    ")
    };
    UpdateAbilityCommand command = new(ability.Id.ToGuid(), payload);
    AbilityModel? result = await _handler.Handle(command, _cancellationToken);
    Assert.NotNull(result);
    Assert.Same(model, result);

    _abilityManager.Verify(x => x.SaveAsync(ability, _cancellationToken), Times.Once());

    Assert.Equal(_actorId, ability.UpdatedBy);
    Assertions.Equal(payload.UniqueName, ability.UniqueName);
    Assert.Equal(displayName, ability.DisplayName);
    Assertions.Equal(payload.Description.Value, ability.Description);
    Assertions.Equal(payload.Link.Value, ability.Link);
    Assertions.Equal(payload.Notes.Value, ability.Notes);
  }
}
