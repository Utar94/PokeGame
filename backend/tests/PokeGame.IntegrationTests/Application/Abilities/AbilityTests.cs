using Logitar;
using Logitar.Portal.Contracts.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application.Abilities.Commands;
using PokeGame.Application.Abilities.Queries;
using PokeGame.Contracts;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities;

[Trait(Traits.Category, Categories.Integration)]
public class AbilityTests : IntegrationTests
{
  private readonly IAbilityRepository _abilityRepository;

  private readonly Ability _static;

  public AbilityTests() : base()
  {
    _abilityRepository = ServiceProvider.GetRequiredService<IAbilityRepository>();

    _static = new Ability(new Name("Static"), UserId);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _abilityRepository.SaveAsync(_static);
  }

  [Fact(DisplayName = "It should create a new ability.")]
  public async Task It_should_create_a_new_ability()
  {
    CreateOrReplaceAbilityPayload payload = new(" Overgrow ")
    {
      Description = "  Powers up Grass-type moves when the Pokémon's HP is low.  ",
      Link = "https://bulbapedia.bulbagarden.net/wiki/Overgrow_(Ability)",
      Notes = "  When a Pokémon with Overgrow uses a Grass-type move, the move's power will be increased by 50% if the user has less than or equal to ⅓ of its maximum HP remaining.\n\nInstead of boosting Grass-type moves' power, Overgrow now technically boosts the Pokémon's Attack or Special Attack by 50% during damage calculation if a Grass-type move is being used, resulting in effectively the same effect.  "
    };
    CreateOrReplaceAbilityCommand command = new(Guid.NewGuid(), payload, Version: null);
    CreateOrReplaceAbilityResult result = await Pipeline.ExecuteAsync(command);
    Assert.True(result.Created);

    AbilityModel? ability = result.Ability;
    Assert.NotNull(ability);
    Assert.Equal(command.Id, ability.Id);
    Assert.Equal(2, ability.Version);
    Assert.Equal(DateTime.UtcNow, ability.CreatedOn, TimeSpan.FromSeconds(1));
    Assert.True(ability.CreatedOn < ability.UpdatedOn);
    Assert.Equal(Actor, ability.CreatedBy);
    Assert.Equal(Actor, ability.UpdatedBy);

    Assert.Equal(payload.Name.Trim(), ability.Name);
    Assert.Equal(payload.Description.CleanTrim(), ability.Description);
    Assert.Equal(payload.Link, ability.Link);
    Assert.Equal(payload.Notes.CleanTrim(), ability.Notes);

    Assert.NotNull(await PokeGameContext.Abilities.AsNoTracking().SingleOrDefaultAsync(x => x.Id == ability.Id));
  }

  [Fact(DisplayName = "It should delete an existing ability.")]
  public async Task It_should_delete_an_existing_ability()
  {
    DeleteAbilityCommand command = new(_static.Id.ToGuid());
    AbilityModel? ability = await Pipeline.ExecuteAsync(command);
    Assert.NotNull(ability);
    Assert.Equal(command.Id, ability.Id);

    Assert.Null(await PokeGameContext.Abilities.AsNoTracking().SingleOrDefaultAsync(x => x.Id == ability.Id));
  }

  [Fact(DisplayName = "It should replace an existing ability.")]
  public async Task It_should_replace_an_existing_ability()
  {
    long version = _static.Version;

    Notes notes = new("When a Pokémon with this Ability is hit by a move that makes contact, there is a 1/3 chance that the attacking Pokémon will become paralyzed. This can affect Ground-type Pokémon.\n\nIf a Pokémon with this Ability is hit by a multistrike move that makes contact, each hit has an independent chance to activate this Ability.");
    _static.Notes = notes;
    _static.Update(UserId);
    await _abilityRepository.SaveAsync(_static);

    CreateOrReplaceAbilityPayload payload = new(" Static ")
    {
      Description = "  The Pokémon is charged with static electricity and may paralyze attackers that make direct contact with it.  ",
      Link = "https://bulbapedia.bulbagarden.net/wiki/Static_(Ability)",
      Notes = "    "
    };
    CreateOrReplaceAbilityCommand command = new(_static.Id.ToGuid(), payload, version);
    CreateOrReplaceAbilityResult result = await Pipeline.ExecuteAsync(command);
    Assert.False(result.Created);

    AbilityModel? ability = result.Ability;
    Assert.NotNull(ability);
    Assert.Equal(command.Id, ability.Id);
    Assert.Equal(_static.Version + 1, ability.Version);
    Assert.Equal(DateTime.UtcNow, ability.UpdatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(Actor, ability.UpdatedBy);

    Assert.Equal(payload.Name.Trim(), ability.Name);
    Assert.Equal(payload.Description?.CleanTrim(), ability.Description);
    Assert.Equal(payload.Link, ability.Link);
    Assert.Equal(notes.Value, ability.Notes);
  }

  [Fact(DisplayName = "It should return empty search results.")]
  public async Task It_should_return_empty_search_results()
  {
    SearchAbilitiesPayload payload = new()
    {
      Search = new TextSearch([new SearchTerm("test")])
    };
    SearchAbilitiesQuery query = new(payload);
    SearchResults<AbilityModel> results = await Pipeline.ExecuteAsync(query);
    Assert.Empty(results.Items);
    Assert.Equal(0, results.Total);
  }

  [Fact(DisplayName = "It should return the correct search results (Kind).")]
  public async Task It_should_return_the_correct_search_results_Kind()
  {
    Ability adaptability = new(new Name("Adaptability"), UserId)
    {
      Kind = AbilityKind.Adaptability
    };
    adaptability.Update(UserId);
    await _abilityRepository.SaveAsync(adaptability);

    SearchAbilitiesPayload payload = new()
    {
      Kind = AbilityKind.Adaptability
    };
    SearchAbilitiesQuery query = new(payload);
    SearchResults<AbilityModel> results = await Pipeline.ExecuteAsync(query);
    Assert.Equal(1, results.Total);
    Assert.Equal(adaptability.Id.ToGuid(), Assert.Single(results.Items).Id);
  }

  [Fact(DisplayName = "It should return the correct search results.")]
  public async Task It_should_return_the_correct_search_results()
  {
    Ability overgrow = new(new Name("Overgrow"), UserId);
    Ability blaze = new(new Name("Blaze"), UserId);
    Ability torrent = new(new Name("Torrent"), UserId);
    await _abilityRepository.SaveAsync([overgrow, blaze, torrent]);

    SearchAbilitiesPayload payload = new()
    {
      Search = new TextSearch([new SearchTerm("%o%"), new SearchTerm("%z%")], SearchOperator.Or),
      Sort = [new AbilitySortOption(AbilitySort.Name, isDescending: true)],
      Skip = 1,
      Limit = 1
    };

    payload.Ids.AddRange((await _abilityRepository.LoadAsync()).Select(ability => ability.Id.ToGuid()));
    payload.Ids.Remove(torrent.Id.ToGuid());
    payload.Ids.Add(Guid.Empty);

    SearchAbilitiesQuery query = new(payload);
    SearchResults<AbilityModel> results = await Pipeline.ExecuteAsync(query);

    Assert.Equal(2, results.Total);
    Assert.Equal(blaze.Id.ToGuid(), Assert.Single(results.Items).Id);
  }

  [Fact(DisplayName = "It should return the ability found by ID.")]
  public async Task It_should_return_the_ability_found_by_Id()
  {
    ReadAbilityQuery query = new(_static.Id.ToGuid());
    AbilityModel? ability = await Pipeline.ExecuteAsync(query);
    Assert.NotNull(ability);
    Assert.Equal(query.Id, ability.Id);
  }

  [Fact(DisplayName = "It should update an existing ability.")]
  public async Task It_should_update_an_existing_ability()
  {
    Notes notes = new("When a Pokémon with this Ability is hit by a move that makes contact, there is a 30% chance that the attacking Pokémon will become paralyzed. This can affect Ground-type Pokémon.\n\nIf a Pokémon with this Ability is hit by a multistrike move that makes contact, each hit has an independent chance to activate this Ability.");
    _static.Notes = notes;
    _static.Update(UserId);
    await _abilityRepository.SaveAsync(_static);

    UpdateAbilityPayload payload = new()
    {
      Description = new Change<string>("  The Pokémon is charged with static electricity and may paralyze attackers that make direct contact with it.  "),
      Link = new Change<string>("https://bulbapedia.bulbagarden.net/wiki/Static_(Ability)")
    };
    UpdateAbilityCommand command = new(_static.Id.ToGuid(), payload);
    AbilityModel? ability = await Pipeline.ExecuteAsync(command);
    Assert.NotNull(ability);

    Assert.Equal(command.Id, ability.Id);
    Assert.Equal(_static.Version + 1, ability.Version);
    Assert.Equal(DateTime.UtcNow, ability.UpdatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(Actor, ability.UpdatedBy);

    Assert.Equal(_static.Name.Value, ability.Name);
    Assert.Equal(payload.Description.Value?.CleanTrim(), ability.Description);
    Assert.Equal(payload.Link.Value, ability.Link);
    Assert.Equal(notes.Value, ability.Notes);
  }
}
