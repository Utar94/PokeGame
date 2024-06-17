using Logitar.Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain.Abilities;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.Application.Abilities.Commands;

[Trait(Traits.Category, Categories.Integration)]
public class DeleteAbilityCommandTests : IntegrationTests
{
  private readonly IAbilityRepository _abilityRepository;

  private readonly AbilityAggregate _ability;

  public DeleteAbilityCommandTests()
  {
    _abilityRepository = ServiceProvider.GetRequiredService<IAbilityRepository>();

    _ability = new(new UniqueNameUnit(AbilityAggregate.UniqueNameSettings, "Static"), ActorId);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _abilityRepository.SaveAsync(_ability);
  }

  [Fact(DisplayName = "It should delete an existing ability.")]
  public async Task It_should_delete_an_existing_ability()
  {
    DeleteAbilityCommand command = new(_ability.Id.ToGuid());
    Ability? ability = await Pipeline.ExecuteAsync(command);
    Assert.NotNull(ability);
    Assert.Equal(_ability.Id.ToGuid(), ability.Id);

    AbilityEntity? entity = await PokemonContext.Abilities.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == _ability.Id.Value);
    Assert.Null(entity);
  }

  [Fact(DisplayName = "It should return null when the ability could not be found.")]
  public async Task It_should_return_null_when_the_ability_could_not_be_found()
  {
    DeleteAbilityCommand command = new(Guid.NewGuid());
    Ability? ability = await Pipeline.ExecuteAsync(command);
    Assert.Null(ability);
  }
}
