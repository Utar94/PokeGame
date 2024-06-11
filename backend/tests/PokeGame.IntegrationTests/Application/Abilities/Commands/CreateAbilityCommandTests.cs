using FluentValidation.Results;
using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;
using PokeGame.Domain.Validators;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.Application.Abilities.Commands;

[Trait(Traits.Category, Categories.Integration)]
public class CreateAbilityCommandTests : IntegrationTests
{
  private readonly IAbilityRepository _abilityRepository;

  public CreateAbilityCommandTests() : base()
  {
    _abilityRepository = ServiceProvider.GetRequiredService<IAbilityRepository>();
  }

  [Fact(DisplayName = "It should create a new ability.")]
  public async Task It_should_create_a_new_ability()
  {
    CreateAbilityPayload payload = new("Overgrow");
    CreateAbilityCommand command = new(payload);
    Ability ability = await Pipeline.ExecuteAsync(command);

    Assert.NotEqual(Guid.Empty, ability.Id);
    Assert.Equal(1, ability.Version);
    Assert.Equal(Actor, ability.CreatedBy);
    Assert.Equal(Actor, ability.UpdatedBy);
    Assert.Equal(ability.CreatedOn, ability.UpdatedOn);

    Assert.Equal(payload.UniqueName, ability.UniqueName);

    AbilityEntity? entity = await PokemonContext.Abilities.AsNoTracking().SingleOrDefaultAsync(x => x.AggregateId == new AggregateId(ability.Id).Value);
    Assert.NotNull(entity);
  }

  [Fact(DisplayName = "It should throw UniqueNameAlreadyUsedException when the unique name is already used.")]
  public async Task It_should_throw_UniqueNameAlreadyUsedException_when_the_unique_name_is_already_used()
  {
    AbilityAggregate ability = new(new UniqueNameUnit(AbilityAggregate.UniqueNameSettings, "Overgrow"));
    await _abilityRepository.SaveAsync(ability);

    CreateAbilityPayload payload = new(ability.UniqueName.Value);
    CreateAbilityCommand command = new(payload);
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<AbilityAggregate>>(async () => await Pipeline.ExecuteAsync(command));
    Assert.Equal(payload.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_is_not_valid()
  {
    CreateAbilityPayload payload = new("Air Lock");
    CreateAbilityCommand command = new(payload);
    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await Pipeline.ExecuteAsync(command));
    ValidationFailure error = Assert.Single(exception.Errors);
    Assert.Equal(nameof(AllowedCharactersValidator), error.ErrorCode);
    Assert.Equal("UniqueName", error.PropertyName);
  }
}
