using Moq;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves;

[Trait(Traits.Category, Categories.Unit)]
public class MoveManagerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IMoveQuerier> _moveQuerier = new();
  private readonly Mock<IMoveRepository> _moveRepository = new();

  private readonly MoveManager _manager;

  private readonly Move _move = new(PokemonType.Normal, MoveCategory.Physical, new UniqueName("Facade"), new PowerPoints(20));

  public MoveManagerTests()
  {
    _manager = new(_moveQuerier.Object, _moveRepository.Object);
  }

  [Fact(DisplayName = "SaveAsync: it should not query moves when the unique did not change.")]
  public async Task Given_NoChange_When_SaveAsync_Then_NotQueried()
  {
    _move.ClearChanges();

    await _manager.SaveAsync(_move, _cancellationToken);

    _moveQuerier.Verify(x => x.FindIdAsync(It.IsAny<UniqueName>(), It.IsAny<CancellationToken>()), Times.Never());
    _moveRepository.Verify(x => x.SaveAsync(_move, _cancellationToken), Times.Once());
  }

  [Fact(DisplayName = "SaveAsync: it should save the move when there is no conflict.")]
  public async Task Given_NoConflict_When_SaveAsync_Then_MoveSaved()
  {
    _moveQuerier.Setup(x => x.FindIdAsync(_move.UniqueName, _cancellationToken)).ReturnsAsync(_move.Id);

    await _manager.SaveAsync(_move, _cancellationToken);

    _moveQuerier.Verify(x => x.FindIdAsync(_move.UniqueName, _cancellationToken), Times.Once());
    _moveRepository.Verify(x => x.SaveAsync(_move, _cancellationToken), Times.Once());
  }

  [Fact(DisplayName = "SaveAsync: it should throw UniqueNameAlreadyUsedException when the unique name is already used.")]
  public async Task Given_UniqueNameConflict_When_SaveAsync_Then_UniqueNameAlreadyUsedException()
  {
    MoveId conflictId = MoveId.NewId();
    _moveQuerier.Setup(x => x.FindIdAsync(_move.UniqueName, _cancellationToken)).ReturnsAsync(conflictId);

    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException>(async () => await _manager.SaveAsync(_move, _cancellationToken));
    Assert.Equal(_move.Id.ToGuid(), exception.EntityId);
    Assert.Equal(conflictId.ToGuid(), exception.ConflictId);
    Assert.Equal(_move.UniqueName.Value, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }
}
