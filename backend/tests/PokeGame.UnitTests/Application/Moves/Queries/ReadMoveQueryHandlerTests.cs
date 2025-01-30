using Moq;
using PokeGame.Application.Moves.Models;

namespace PokeGame.Application.Moves.Queries;

[Trait(Traits.Category, Categories.Unit)]
public class ReadMoveQueryHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IMoveQuerier> _moveQuerier = new();

  private readonly ReadMoveQueryHandler _handler;

  private readonly MoveModel _tackle = new()
  {
    Id = Guid.NewGuid(),
    UniqueName = "Tackle"
  };
  private readonly MoveModel _leer = new()
  {
    Id = Guid.NewGuid(),
    UniqueName = "Leer"
  };

  public ReadMoveQueryHandlerTests()
  {
    _handler = new(_moveQuerier.Object);

    _moveQuerier.Setup(x => x.ReadAsync(_tackle.Id, _cancellationToken)).ReturnsAsync(_tackle);
    _moveQuerier.Setup(x => x.ReadAsync(_tackle.UniqueName, _cancellationToken)).ReturnsAsync(_tackle);
    _moveQuerier.Setup(x => x.ReadAsync(_leer.Id, _cancellationToken)).ReturnsAsync(_leer);
    _moveQuerier.Setup(x => x.ReadAsync(_leer.UniqueName, _cancellationToken)).ReturnsAsync(_leer);
  }

  [Fact(DisplayName = "It should return null when no move was found.")]
  public async Task Given_NoneFound_When_Handle_Then_NullReturned()
  {
    ReadMoveQuery query = new(Id: null, UniqueName: null);
    MoveModel? move = await _handler.Handle(query, _cancellationToken);
    Assert.Null(move);
  }

  [Fact(DisplayName = "It should return the move found by ID.")]
  public async Task Given_FoundById_When_Handle_Then_Returned()
  {
    ReadMoveQuery query = new(_tackle.Id, _tackle.UniqueName);
    MoveModel? move = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(move);
    Assert.Same(_tackle, move);
  }

  [Fact(DisplayName = "It should return the move found by unique name.")]
  public async Task Given_FoundByUniqueName_When_Handle_Then_Returned()
  {
    ReadMoveQuery query = new(Guid.Empty, _tackle.UniqueName);
    MoveModel? move = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(move);
    Assert.Same(_tackle, move);
  }

  [Fact(DisplayName = "It should throw TooManyResultsException when more than one move were found.")]
  public async Task Given_ManyFound_When_Handle_Then_TooManyResultsException()
  {
    ReadMoveQuery query = new(_tackle.Id, _leer.UniqueName);
    var exception = await Assert.ThrowsAsync<TooManyResultsException<MoveModel>>(async () => await _handler.Handle(query, _cancellationToken));
    Assert.Equal(1, exception.ExpectedCount);
    Assert.Equal(2, exception.ActualCount);
  }
}
