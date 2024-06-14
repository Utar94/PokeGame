using FluentValidation.Results;
using Logitar;
using Logitar.Identity.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

[Trait(Traits.Category, Categories.Integration)]
public class ReplaceAbilityCommandTests : IntegrationTests
{
  private readonly IAbilityRepository _abilityRepository;

  private readonly AbilityAggregate _ability;

  public ReplaceAbilityCommandTests() : base()
  {
    _abilityRepository = ServiceProvider.GetRequiredService<IAbilityRepository>();

    _ability = new(new UniqueNameUnit(AbilityAggregate.UniqueNameSettings, "Overgrow"), ActorId);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _abilityRepository.SaveAsync(_ability);
  }

  [Fact(DisplayName = "It should replace an existing ability.")]
  public async Task It_should_replace_an_existing_ability()
  {
    long version = _ability.Version;

    _ability.Description = new DescriptionUnit("Powers up Grass-type moves when the Pokémon's HP is low.");
    _ability.Update(ActorId);
    await _abilityRepository.SaveAsync(_ability);

    ReplaceAbilityPayload payload = new("Overgrow")
    {
      DisplayName = "  Overgrow  ",
      Description = "    ",
      Reference = "https://bulbapedia.bulbagarden.net/wiki/Overgrow_(Ability)",
      Notes = "  Instead of boosting Grass-type moves' power, Overgrow now technically boosts the Pokémon's Attack or Special Attack by 50% during damage calculation if a Grass-type move is being used, resulting in effectively the same effect.  "
    };
    ReplaceAbilityCommand command = new(_ability.Id.ToGuid(), payload, version);
    Ability? ability = await Pipeline.ExecuteAsync(command);
    Assert.NotNull(ability);

    Assert.Equal(_ability.Id.ToGuid(), ability.Id);
    Assert.Equal(_ability.Version + 1, ability.Version);
    Assert.Equal(Actor, ability.CreatedBy);
    Assert.Equal(Actor, ability.UpdatedBy);
    Assert.True(ability.CreatedOn < ability.UpdatedOn);

    Assert.Equal(payload.UniqueName, ability.UniqueName);
    Assert.Equal(payload.DisplayName.CleanTrim(), ability.DisplayName);
    Assert.Equal(_ability.Description.Value, ability.Description);
    Assert.Equal(payload.Reference.CleanTrim(), ability.Reference);
    Assert.Equal(payload.Notes.CleanTrim(), ability.Notes);
  }

  [Fact(DisplayName = "It should return null when the ability could not be found.")]
  public async Task It_should_return_null_when_the_ability_could_not_be_found()
  {
    ReplaceAbilityPayload payload = new("Overgrow");
    ReplaceAbilityCommand command = new(Id: Guid.NewGuid(), payload, Version: null);
    Assert.Null(await Pipeline.ExecuteAsync(command));
  }

  [Fact(DisplayName = "It should throw UniqueNameAlreadyUsedException when the unique name is already used.")]
  public async Task It_should_throw_UniqueNameAlreadyUsedException_when_the_unique_name_is_already_used()
  {
    AbilityAggregate ability = new(new UniqueNameUnit(AbilityAggregate.UniqueNameSettings, "Blaze"), ActorId);
    await _abilityRepository.SaveAsync(ability);

    ReplaceAbilityPayload payload = new(ability.UniqueName.Value);
    ReplaceAbilityCommand command = new(_ability.Id.ToGuid(), payload, Version: null);
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<AbilityAggregate>>(async () => await Pipeline.ExecuteAsync(command));
    Assert.Equal(payload.UniqueName, exception.UniqueName);
    Assert.Equal(nameof(payload.UniqueName), exception.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_is_not_valid()
  {
    ReplaceAbilityPayload payload = new("Overgrow!");
    ReplaceAbilityCommand command = new(_ability.Id.ToGuid(), payload, Version: null);
    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await Pipeline.ExecuteAsync(command));
    ValidationFailure error = Assert.Single(exception.Errors);
    Assert.Equal(nameof(AllowedCharactersValidator), error.ErrorCode);
    Assert.Equal("UniqueName", error.PropertyName);
  }
}
