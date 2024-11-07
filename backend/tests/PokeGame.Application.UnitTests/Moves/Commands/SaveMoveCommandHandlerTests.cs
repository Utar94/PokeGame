using Logitar;
using Moq;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class SaveMoveCommandHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IMoveQuerier> _moveQuerier = new();
  private readonly Mock<IMoveRepository> _moveRepository = new();

  private readonly SaveMoveCommandHandler _handler;

  private readonly Move _move = new(PokemonType.Normal, MoveCategory.Physical, new UniqueName("tackle"), UserId.NewId());

  public SaveMoveCommandHandlerTests()
  {
    _handler = new(_moveQuerier.Object, _moveRepository.Object);
  }

  [Fact(DisplayName = "It should save a move without changes.")]
  public async Task It_should_save_a_move_without_changes()
  {
    _move.ClearChanges();

    SaveMoveCommand command = new(_move);
    await _handler.Handle(command, _cancellationToken);

    _moveQuerier.Verify(x => x.FindIdAsync(It.IsAny<UniqueName>(), It.IsAny<CancellationToken>()), Times.Never);
    _moveRepository.Verify(x => x.SaveAsync(_move, _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should save the move.")]
  public async Task It_should_save_the_move()
  {
    _moveQuerier.Setup(x => x.FindIdAsync(_move.UniqueName, _cancellationToken)).ReturnsAsync(_move.Id);

    SaveMoveCommand command = new(_move);
    await _handler.Handle(command, _cancellationToken);

    _moveQuerier.Verify(x => x.FindIdAsync(_move.UniqueName, _cancellationToken), Times.Once);
    _moveRepository.Verify(x => x.SaveAsync(_move, _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should throw UniqueNameAlreadyUsedException when the unique name is already used.")]
  public async Task It_should_throw_UniqueNameAlreadyUsedException_when_the_unique_name_is_already_used()
  {
    Move move = new(_move.Type, _move.Category, _move.UniqueName, UserId.NewId());
    _moveQuerier.Setup(x => x.FindIdAsync(_move.UniqueName, _cancellationToken)).ReturnsAsync(move.Id);

    SaveMoveCommand command = new(_move);
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException>(async () => await _handler.Handle(command, _cancellationToken));
    Assert.Equal(typeof(Move).GetNamespaceQualifiedName(), exception.TypeName);
    Assert.Equal(move.UniqueName.Value, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
    Assert.Equal([_move.Id.ToGuid(), move.Id.ToGuid()], exception.ConflictingIds);
  }
}

