using Moq;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class DeleteMoveCommandHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IMoveQuerier> _moveQuerier = new();
  private readonly Mock<IMoveRepository> _moveRepository = new();

  private readonly DeleteMoveCommandHandler _handler;

  private readonly UserId _userId = UserId.NewId();
  private readonly Move _move;

  public DeleteMoveCommandHandlerTests()
  {
    _handler = new(_moveQuerier.Object, _moveRepository.Object);

    _move = new(PokemonType.Normal, MoveCategory.Status, new UniqueName("growl"), _userId);
    _moveRepository.Setup(x => x.LoadAsync(_move.Id, _cancellationToken)).ReturnsAsync(_move);
  }

  [Fact(DisplayName = "It should delete an existing move.")]
  public async Task It_should_delete_an_existing_move()
  {
    DeleteMoveCommand command = new(_move.Id.ToGuid());
    command.Contextualize();

    MoveModel model = new();
    _moveQuerier.Setup(x => x.ReadAsync(_move, _cancellationToken)).ReturnsAsync(model);

    MoveModel? result = await _handler.Handle(command, _cancellationToken);
    Assert.NotNull(result);
    Assert.Same(model, result);

    _moveRepository.Verify(x => x.SaveAsync(It.Is<Move>(y => y.Equals(_move) && y.IsDeleted), _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should return null when the move could not be found.")]
  public async Task It_should_return_null_when_the_move_could_not_be_found()
  {
    DeleteMoveCommand command = new(Guid.NewGuid());
    command.Contextualize();

    Assert.Null(await _handler.Handle(command, _cancellationToken));
  }
}
