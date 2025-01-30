using Logitar.EventSourcing;
using PokeGame.Domain;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Moves.Events;

namespace PokeGame.Application.Moves;

internal class MoveManager : IMoveManager
{
  private readonly IMoveQuerier _moveQuerier;
  private readonly IMoveRepository _moveRepository;

  public MoveManager(IMoveQuerier moveQuerier, IMoveRepository moveRepository)
  {
    _moveQuerier = moveQuerier;
    _moveRepository = moveRepository;
  }

  public async Task SaveAsync(Move move, CancellationToken cancellationToken)
  {
    UniqueName? uniqueName = null;
    foreach (IEvent change in move.Changes)
    {
      if (change is MoveCreated created)
      {
        uniqueName = created.UniqueName;
      }
      else if (change is MoveUpdated updated && updated.UniqueName != null)
      {
        uniqueName = updated.UniqueName;
      }
    }

    if (uniqueName != null)
    {
      MoveId? conflictId = await _moveQuerier.FindIdAsync(uniqueName, cancellationToken);
      if (conflictId.HasValue && !conflictId.Value.Equals(move.Id))
      {
        throw new UniqueNameAlreadyUsedException(move, conflictId.Value);
      }
    }

    await _moveRepository.SaveAsync(move, cancellationToken);
  }
}
