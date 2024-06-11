using Logitar.Identity.Contracts.Settings;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Queries;

[Trait(Traits.Category, Categories.Integration)]
public class ReadAbilityQueryTests : IntegrationTests
{
  private readonly IAbilityRepository _abilityRepository;

  private readonly AbilityAggregate _blaze;
  private readonly AbilityAggregate _overgrow;

  public ReadAbilityQueryTests() : base()
  {
    _abilityRepository = ServiceProvider.GetRequiredService<IAbilityRepository>();

    IUniqueNameSettings uniqueNameSettings = AbilityAggregate.UniqueNameSettings;
    _blaze = new(new UniqueNameUnit(uniqueNameSettings, "Blaze"));
    _overgrow = new(new UniqueNameUnit(uniqueNameSettings, "Overgrow"));
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _abilityRepository.SaveAsync([_blaze, _overgrow]);
  }

  [Fact(DisplayName = "It should return null when the ability is not found.")]
  public async Task It_should_return_null_when_the_ability_is_not_found()
  {
    ReadAbilityQuery query = new(Id: Guid.NewGuid(), UniqueName: "Torrent");
    Assert.Null(await Pipeline.ExecuteAsync(query));
  }

  [Fact(DisplayName = "It should return the ability found by ID.")]
  public async Task It_should_return_the_ability_found_by_Id()
  {
    ReadAbilityQuery query = new(_overgrow.Id.ToGuid(), UniqueName: null);
    Ability? ability = await Pipeline.ExecuteAsync(query);
    Assert.NotNull(ability);
    Assert.Equal(_overgrow.Id.ToGuid(), ability.Id);
  }

  [Fact(DisplayName = "It should return the ability found by unique name.")]
  public async Task It_should_return_the_ability_found_by_unique_name()
  {
    ReadAbilityQuery query = new(Id: null, _blaze.UniqueName.Value);
    Ability? ability = await Pipeline.ExecuteAsync(query);
    Assert.NotNull(ability);
    Assert.Equal(_blaze.Id.ToGuid(), ability.Id);
  }

  [Fact(DisplayName = "It should throw TooManyResultsException when many abilities are found.")]
  public async Task It_should_throw_TooManyResultsException_when_many_abilities_are_found()
  {
    ReadAbilityQuery query = new(_overgrow.Id.ToGuid(), _blaze.UniqueName.Value);
    var exception = await Assert.ThrowsAsync<TooManyResultsException<Ability>>(async () => await Pipeline.ExecuteAsync(query));
    Assert.Equal(1, exception.ExpectedCount);
    Assert.Equal(2, exception.ActualCount);
  }
}
