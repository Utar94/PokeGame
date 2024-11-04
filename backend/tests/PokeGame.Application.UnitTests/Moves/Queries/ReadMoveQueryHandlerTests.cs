using Moq;
using PokeGame.Contracts.Moves;

namespace PokeGame.Application.Moves.Queries;

[Trait(Traits.Category, Categories.Unit)]
public class ReadMoveQueryHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IMoveQuerier> _moveQuerier = new();

  private readonly ReadMoveQueryHandler _handler;

  private readonly MoveModel _move = new("Growl")
  {
    Id = Guid.NewGuid()
  };

  public ReadMoveQueryHandlerTests()
  {
    _handler = new(_moveQuerier.Object);

    _moveQuerier.Setup(x => x.ReadAsync(_move.Id, _cancellationToken)).ReturnsAsync(_move);
  }

  [Fact(DisplayName = "It should return null when no move was found.")]
  public async Task It_should_return_null_when_no_move_was_found()
  {
    ReadMoveQuery query = new(Guid.NewGuid());
    query.Contextualize();

    Assert.Null(await _handler.Handle(query, _cancellationToken));
  }

  [Fact(DisplayName = "It should return the move found by ID.")]
  public async Task It_should_return_the_move_found_by_Id()
  {
    ReadMoveQuery query = new(_move.Id);
    query.Contextualize();

    MoveModel? move = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(move);
    Assert.Same(_move, move);
  }
}
