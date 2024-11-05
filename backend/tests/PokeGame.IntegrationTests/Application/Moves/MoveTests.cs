using Logitar;
using Logitar.Portal.Contracts.Search;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application.Moves.Commands;
using PokeGame.Application.Moves.Queries;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves;

[Trait(Traits.Category, Categories.Integration)]
public class MoveTests : IntegrationTests
{
  private readonly IMoveRepository _moveRepository;

  private readonly Move _thunderShock;

  public MoveTests() : base()
  {
    _moveRepository = ServiceProvider.GetRequiredService<IMoveRepository>();

    _thunderShock = new Move(PokemonType.Electric, MoveCategory.Special, new Name("Thunder Shock"), UserId);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _moveRepository.SaveAsync(_thunderShock);
  }

  [Fact(DisplayName = "It should create a new move.")]
  public async Task It_should_create_a_new_move()
  {
    CreateOrReplaceMovePayload payload = new(" Facade ")
    {
      Type = PokemonType.Normal,
      Category = MoveCategory.Physical,
      Kind = MoveKind.Facade,
      Description = "  This move's power is doubled if the user is poisoned, burned, or paralyzed.  ",
      Accuracy = 100,
      Power = 70,
      PowerPoints = 20,
      StatisticChanges = [new StatisticChangeModel(BattleStatistic.Attack, stages: 2)],
      Status = new InflictedConditionModel(StatusCondition.Burn, chance: 1),
      VolatileConditions = ["Facade"],
      Link = "https://bulbapedia.bulbagarden.net/wiki/Facade_(move)",
      Notes = "  When using Facade, Burn's effect of halving the damage done by physical moves is now ignored.  "
    };
    CreateOrReplaceMoveCommand command = new(Guid.NewGuid(), payload, Version: null);
    CreateOrReplaceMoveResult result = await Pipeline.ExecuteAsync(command);
    Assert.True(result.Created);

    MoveModel? move = result.Move;
    Assert.NotNull(move);
    Assert.Equal(command.Id, move.Id);
    Assert.Equal(2, move.Version);
    Assert.Equal(DateTime.UtcNow, move.CreatedOn, TimeSpan.FromSeconds(1));
    Assert.True(move.CreatedOn < move.UpdatedOn);
    Assert.Equal(Actor, move.CreatedBy);
    Assert.Equal(Actor, move.UpdatedBy);

    Assert.Equal(payload.Type, move.Type);
    Assert.Equal(payload.Category, move.Category);
    Assert.Equal(payload.Kind, move.Kind);
    Assert.Equal(payload.Name.Trim(), move.Name);
    Assert.Equal(payload.Description.CleanTrim(), move.Description);
    Assert.Equal(payload.Accuracy, move.Accuracy);
    Assert.Equal(payload.Power, move.Power);
    Assert.Equal(payload.PowerPoints, move.PowerPoints);
    Assert.Equal(payload.StatisticChanges, move.StatisticChanges);
    Assert.Equal(payload.Status, move.Status);
    Assert.Equal(payload.VolatileConditions, move.VolatileConditions);
    Assert.Equal(payload.Link, move.Link);
    Assert.Equal(payload.Notes.CleanTrim(), move.Notes);

    //Assert.NotNull(await PokeGameContext.Moves.AsNoTracking().SingleOrDefaultAsync(x => x.Id == move.Id)); // TODO(fpion): complete
  }

  [Fact(DisplayName = "It should delete an existing move.")]
  public async Task It_should_delete_an_existing_move()
  {
    DeleteMoveCommand command = new(_thunderShock.Id.ToGuid());
    MoveModel? move = await Pipeline.ExecuteAsync(command);
    Assert.NotNull(move);
    Assert.Equal(command.Id, move.Id);

    //Assert.Null(await PokeGameContext.Moves.AsNoTracking().SingleOrDefaultAsync(x => x.Id == move.Id)); // TODO(fpion): complete
  }

  [Fact(DisplayName = "It should replace an existing move.")]
  public async Task It_should_replace_an_existing_move()
  {
    long version = _thunderShock.Version;

    Description description = new("The user attacks the target with a jolt of electricity. This may also leave the target with paralysis.");
    _thunderShock.Description = description;
    _thunderShock.Update(UserId);
    await _moveRepository.SaveAsync(_thunderShock);

    CreateOrReplaceMovePayload payload = new(" Thunder Shock ")
    {
      Type = PokemonType.Water,
      Category = MoveCategory.Status,
      Description = "    ",
      Accuracy = 100,
      Power = 40,
      PowerPoints = 30,
      StatisticChanges = [new StatisticChangeModel(BattleStatistic.Speed, stages: -1)],
      Status = new InflictedConditionModel(StatusCondition.Paralysis, chance: 10),
      VolatileConditions = ["Thunder Shock"],
      Link = "https://bulbapedia.bulbagarden.net/wiki/Thunder_Shock_(move)",
      Notes = "    "
    };
    CreateOrReplaceMoveCommand command = new(_thunderShock.Id.ToGuid(), payload, version);
    CreateOrReplaceMoveResult result = await Pipeline.ExecuteAsync(command);
    Assert.False(result.Created);

    MoveModel? move = result.Move;
    Assert.NotNull(move);
    Assert.Equal(command.Id, move.Id);
    Assert.Equal(_thunderShock.Version + 1, move.Version);
    Assert.Equal(DateTime.UtcNow, move.UpdatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(Actor, move.UpdatedBy);

    Assert.Equal(_thunderShock.Type, move.Type);
    Assert.Equal(_thunderShock.Category, move.Category);
    Assert.Equal(payload.Kind, move.Kind);
    Assert.Equal(payload.Name.Trim(), move.Name);
    Assert.Equal(description.Value, move.Description);
    Assert.Equal(payload.Accuracy, move.Accuracy);
    Assert.Equal(payload.Power, move.Power);
    Assert.Equal(payload.PowerPoints, move.PowerPoints);
    Assert.Equal(payload.StatisticChanges, move.StatisticChanges);
    Assert.Equal(payload.Status, move.Status);
    Assert.Equal(payload.VolatileConditions, move.VolatileConditions);
    Assert.Equal(payload.Link, move.Link);
    Assert.Equal(payload.Notes.CleanTrim(), move.Notes);
  }

  [Fact(DisplayName = "It should return empty search results.")]
  public async Task It_should_return_empty_search_results()
  {
    SearchMovesPayload payload = new()
    {
      Search = new TextSearch([new SearchTerm("test")])
    };
    SearchMovesQuery query = new(payload);
    SearchResults<MoveModel> results = await Pipeline.ExecuteAsync(query);
    Assert.Empty(results.Items);
    Assert.Equal(0, results.Total);
  }

  [Fact(DisplayName = "It should return the correct search results (Kind).")]
  public async Task It_should_return_the_correct_search_results_Kind()
  {
    Move facade = new(PokemonType.Normal, MoveCategory.Physical, new Name("Facade"), UserId)
    {
      Kind = MoveKind.Facade
    };
    facade.Update(UserId);
    await _moveRepository.SaveAsync(facade);

    SearchMovesPayload payload = new()
    {
      Kind = MoveKind.Facade
    };
    SearchMovesQuery query = new(payload);
    SearchResults<MoveModel> results = await Pipeline.ExecuteAsync(query);
    Assert.Equal(1, results.Total);
    Assert.Equal(facade.Id.ToGuid(), Assert.Single(results.Items).Id);
  }

  [Fact(DisplayName = "It should return the correct search results.")]
  public async Task It_should_return_the_correct_search_results()
  {
    Move focusEnergy = new(PokemonType.Normal, MoveCategory.Status, new Name("Focus Energy"), UserId)
    {
      PowerPoints = 30
    };
    focusEnergy.Update(UserId);
    Move growl = new(PokemonType.Normal, MoveCategory.Status, new Name("Growl"), UserId)
    {
      Accuracy = 100,
      PowerPoints = 40
    };
    growl.Update(UserId);
    Move playNice = new(PokemonType.Normal, MoveCategory.Status, new Name("Play Nice"), UserId)
    {
      PowerPoints = 20
    };
    playNice.Update(UserId);
    Move sweetKiss = new(PokemonType.Fairy, MoveCategory.Status, new Name("Sweet Kiss"), UserId)
    {
      Accuracy = 75,
      PowerPoints = 10
    };
    sweetKiss.Update(UserId);
    Move tackle = new(PokemonType.Normal, MoveCategory.Physical, new Name("Tackle"), UserId)
    {
      Accuracy = 100,
      Power = 40,
      PowerPoints = 35
    };
    tackle.Update(UserId);
    Move tailWhip = new(PokemonType.Normal, MoveCategory.Status, new Name("Tail Whip"), UserId)
    {
      Accuracy = 100,
      PowerPoints = 30
    };
    tailWhip.Update(UserId);

    await _moveRepository.SaveAsync([growl, playNice, sweetKiss, tackle, tailWhip]);

    SearchMovesPayload payload = new()
    {
      Type = PokemonType.Normal,
      Category = MoveCategory.Status,
      Search = new TextSearch([new SearchTerm("%kiss"), new SearchTerm("%o%"), new SearchTerm("ta%")], SearchOperator.Or),
      Sort = [new MoveSortOption(MoveSort.PowerPoints, isDescending: true)],
      Skip = 1,
      Limit = 1
    };

    payload.Ids.AddRange((await _moveRepository.LoadAsync()).Select(move => move.Id.ToGuid()));
    payload.Ids.Remove(focusEnergy.Id.ToGuid());
    payload.Ids.Add(Guid.Empty);

    SearchMovesQuery query = new(payload);
    SearchResults<MoveModel> results = await Pipeline.ExecuteAsync(query);

    Assert.Equal(2, results.Total);
    Assert.Equal(tailWhip.Id.ToGuid(), Assert.Single(results.Items).Id);
  }

  [Fact(DisplayName = "It should return the move found by ID.")]
  public async Task It_should_return_the_move_found_by_Id()
  {
    ReadMoveQuery query = new(_thunderShock.Id.ToGuid());
    MoveModel? move = await Pipeline.ExecuteAsync(query);
    Assert.NotNull(move);
    Assert.Equal(query.Id, move.Id);
  }

  [Fact(DisplayName = "It should update an existing move.")]
  public async Task It_should_update_an_existing_move()
  {
    Description description = new("The user attacks the target with a jolt of electricity. This may also leave the target with paralysis.");
    _thunderShock.Description = description;
    _thunderShock.Notes = new Notes("[…]");
    _thunderShock.AddVolatileCondition(new VolatileCondition("Curse"));
    _thunderShock.Update(UserId);
    await _moveRepository.SaveAsync(_thunderShock);

    UpdateMovePayload payload = new()
    {
      Accuracy = new Change<int?>(100),
      Power = new Change<int?>(40),
      PowerPoints = 30,
      StatisticChanges = [new StatisticChangeModel(BattleStatistic.Speed, stages: -1)],
      Status = new Change<InflictedConditionModel>(new InflictedConditionModel(StatusCondition.Paralysis, chance: 10)),
      VolatileConditions =
      [
        new VolatileConditionUpdate("Thunder Shock", ActionKind.Add),
        new VolatileConditionUpdate("Curse", ActionKind.Remove)
      ],
      Link = new Change<string>("https://bulbapedia.bulbagarden.net/wiki/Thunder_Shock_(move)"),
      Notes = new Change<string>("    ")
    };
    UpdateMoveCommand command = new(_thunderShock.Id.ToGuid(), payload);
    MoveModel? move = await Pipeline.ExecuteAsync(command);
    Assert.NotNull(move);

    Assert.Equal(command.Id, move.Id);
    Assert.Equal(_thunderShock.Version + 1, move.Version);
    Assert.Equal(DateTime.UtcNow, move.UpdatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(Actor, move.UpdatedBy);

    Assert.Equal(_thunderShock.Type, move.Type);
    Assert.Equal(_thunderShock.Category, move.Category);
    Assert.Equal(payload.Kind?.Value, move.Kind);
    Assert.Equal(_thunderShock.Name.Value, move.Name);
    Assert.Equal(description.Value, move.Description);
    Assert.Equal(payload.Accuracy.Value, move.Accuracy);
    Assert.Equal(payload.Power.Value, move.Power);
    Assert.Equal(payload.PowerPoints, move.PowerPoints);

    StatisticChangeModel change = Assert.Single(move.StatisticChanges);
    Assert.Equal(BattleStatistic.Speed, change.Statistic);
    Assert.Equal(-1, change.Stages);

    Assert.Equal(payload.Status.Value, move.Status);
    Assert.Equal("Thunder Shock", Assert.Single(move.VolatileConditions));
    Assert.Equal(payload.Link.Value, move.Link);
    Assert.Equal(payload.Notes?.Value?.CleanTrim(), move.Notes);
  }
}
