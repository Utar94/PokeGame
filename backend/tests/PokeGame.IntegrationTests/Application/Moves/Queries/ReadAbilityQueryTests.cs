using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Queries;

[Trait(Traits.Category, Categories.Integration)]
public class ReadMoveQueryTests : IntegrationTests
{
  private readonly IMoveRepository _moveRepository;

  private readonly MoveAggregate _growl;
  private readonly MoveAggregate _tackle;

  public ReadMoveQueryTests() : base()
  {
    _moveRepository = ServiceProvider.GetRequiredService<IMoveRepository>();

    IUniqueNameSettings uniqueNameSettings = MoveAggregate.UniqueNameSettings;
    _growl = new(PokemonType.Normal, MoveCategory.Status, new UniqueNameUnit(uniqueNameSettings, "Growl"));
    _tackle = new(PokemonType.Normal, MoveCategory.Physical, new UniqueNameUnit(uniqueNameSettings, "Tackle"));
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _moveRepository.SaveAsync([_growl, _tackle]);
  }

  [Fact(DisplayName = "It should return null when the move is not found.")]
  public async Task It_should_return_null_when_the_move_is_not_found()
  {
    ReadMoveQuery query = new(Id: Guid.NewGuid(), UniqueName: "Leafage");
    Assert.Null(await Pipeline.ExecuteAsync(query));
  }

  [Fact(DisplayName = "It should return the move found by ID.")]
  public async Task It_should_return_the_move_found_by_Id()
  {
    ReadMoveQuery query = new(_tackle.Id.ToGuid(), UniqueName: null);
    Move? move = await Pipeline.ExecuteAsync(query);
    Assert.NotNull(move);
    Assert.Equal(_tackle.Id.ToGuid(), move.Id);
  }

  [Fact(DisplayName = "It should return the move found by unique name.")]
  public async Task It_should_return_the_move_found_by_unique_name()
  {
    ReadMoveQuery query = new(Id: null, _growl.UniqueName.Value);
    Move? move = await Pipeline.ExecuteAsync(query);
    Assert.NotNull(move);
    Assert.Equal(_growl.Id.ToGuid(), move.Id);
  }

  [Fact(DisplayName = "It should throw TooManyResultsException when many moves are found.")]
  public async Task It_should_throw_TooManyResultsException_when_many_moves_are_found()
  {
    ReadMoveQuery query = new(_tackle.Id.ToGuid(), _growl.UniqueName.Value);
    var exception = await Assert.ThrowsAsync<TooManyResultsException<Move>>(async () => await Pipeline.ExecuteAsync(query));
    Assert.Equal(1, exception.ExpectedCount);
    Assert.Equal(2, exception.ActualCount);
  }
}
