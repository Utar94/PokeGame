using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Portal.Contracts.Search;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Queries;

[Trait(Traits.Category, Categories.Integration)]
public class SearchMovesQueryTests : IntegrationTests
{
  private readonly IMoveRepository _moveRepository;

  private readonly MoveAggregate _growl;
  private readonly MoveAggregate _leer;
  private readonly MoveAggregate _smokescreen;
  private readonly MoveAggregate _synthesis;
  private readonly MoveAggregate _tackle;
  private readonly MoveAggregate _tailWhip;

  public SearchMovesQueryTests() : base()
  {
    _moveRepository = ServiceProvider.GetRequiredService<IMoveRepository>();

    IUniqueNameSettings uniqueNameSettings = MoveAggregate.UniqueNameSettings;
    _growl = new(PokemonType.Normal, MoveCategory.Status, new UniqueNameUnit(uniqueNameSettings, "Growl"));
    _leer = new(PokemonType.Normal, MoveCategory.Status, new UniqueNameUnit(uniqueNameSettings, "Leer"));
    _smokescreen = new(PokemonType.Normal, MoveCategory.Status, new UniqueNameUnit(uniqueNameSettings, "Smokescreen"));
    _synthesis = new(PokemonType.Grass, MoveCategory.Status, new UniqueNameUnit(uniqueNameSettings, "Synthesis"));
    _tackle = new(PokemonType.Normal, MoveCategory.Physical, new UniqueNameUnit(uniqueNameSettings, "Tackle"));
    _tailWhip = new(PokemonType.Normal, MoveCategory.Status, new UniqueNameUnit(uniqueNameSettings, "TailWhip"));
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _moveRepository.SaveAsync([_growl, _leer, _smokescreen, _synthesis, _tackle, _tailWhip]);
  }

  [Fact(DisplayName = "It should return empty search results.")]
  public async Task It_should_return_empty_search_results()
  {
    SearchMovesPayload payload = new()
    {
      Type = PokemonType.Fighting
    };

    SearchMovesQuery query = new(payload);
    SearchResults<Move> results = await Pipeline.ExecuteAsync(query);
    Assert.Equal(0, results.Total);
    Assert.Empty(results.Items);
  }

  [Fact(DisplayName = "It should return the correct search results.")]
  public async Task It_should_return_the_correct_search_results()
  {
    SearchMovesPayload payload = new()
    {
      Type = PokemonType.Normal,
      Category = MoveCategory.Status,
      Ids = (await _moveRepository.LoadAsync()).Select(x => x.Id.ToGuid()).ToList(),
      Search = new TextSearch([new SearchTerm("%i%"), new SearchTerm("%l%")], SearchOperator.Or),
      Sort = [new MoveSortOption(MoveSort.UniqueName, isDescending: false)],
      Skip = 1,
      Limit = 1
    };

    payload.Ids.Remove(_leer.Id.ToGuid());
    payload.Ids.Add(Guid.Empty);

    SearchMovesQuery query = new(payload);
    SearchResults<Move> results = await Pipeline.ExecuteAsync(query);
    Assert.Equal(2, results.Total);
    Assert.Equal(payload.Limit, results.Items.Count);
    Assert.Equal(_tailWhip.Id.ToGuid(), results.Items.Single().Id);
  }
}
