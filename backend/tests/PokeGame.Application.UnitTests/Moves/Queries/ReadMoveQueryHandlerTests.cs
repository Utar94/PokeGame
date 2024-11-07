using Moq;
using PokeGame.Contracts.Moves;

namespace PokeGame.Application.Moves.Queries;

[Trait(Traits.Category, Categories.Unit)]
public class ReadMoveQueryHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IMoveQuerier> _moveQuerier = new();

  private readonly ReadMoveQueryHandler _handler;

  private readonly MoveModel _growl = new("growl")
  {
    Id = Guid.NewGuid()
  };
  private readonly MoveModel _tackle = new("tackle")
  {
    Id = Guid.NewGuid()
  };

  public ReadMoveQueryHandlerTests()
  {
    _handler = new(_moveQuerier.Object);

    _moveQuerier.Setup(x => x.ReadAsync(_growl.Id, _cancellationToken)).ReturnsAsync(_growl);
    _moveQuerier.Setup(x => x.ReadAsync(_growl.UniqueName, _cancellationToken)).ReturnsAsync(_growl);
    _moveQuerier.Setup(x => x.ReadAsync(_tackle.Id, _cancellationToken)).ReturnsAsync(_tackle);
    _moveQuerier.Setup(x => x.ReadAsync(_tackle.UniqueName, _cancellationToken)).ReturnsAsync(_tackle);
  }

  [Fact(DisplayName = "It should return null when no move was found.")]
  public async Task It_should_return_null_when_no_move_was_found()
  {
    ReadMoveQuery query = new(Guid.NewGuid(), UniqueName: "thunder-shock");
    query.Contextualize();

    Assert.Null(await _handler.Handle(query, _cancellationToken));
  }

  [Fact(DisplayName = "It should return the move found by ID.")]
  public async Task It_should_return_the_move_found_by_Id()
  {
    ReadMoveQuery query = new(_tackle.Id, UniqueName: "thunder-shock");
    query.Contextualize();

    MoveModel? move = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(move);
    Assert.Same(_tackle, move);
  }

  [Fact(DisplayName = "It should return the move found by unique name.")]
  public async Task It_should_return_the_move_found_by_unique_name()
  {
    ReadMoveQuery query = new(Guid.NewGuid(), _growl.UniqueName);
    query.Contextualize();

    MoveModel? move = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(move);
    Assert.Same(_growl, move);
  }

  [Fact(DisplayName = "It should throw TooManyResultsException when more than one moves were found.")]
  public async Task It_should_throw_TooManyResultsException_when_more_than_one_moves_were_found()
  {
    ReadMoveQuery query = new(_tackle.Id, _growl.UniqueName);
    query.Contextualize();

    var exception = await Assert.ThrowsAsync<TooManyResultsException<MoveModel>>(async () => await _handler.Handle(query, _cancellationToken));
    Assert.Equal(1, exception.ExpectedCount);
    Assert.Equal(2, exception.ActualCount);
  }
}
