using FluentValidation;
using MediatR;
using PokeGame.Application.Moves.Validators;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

public record UpdateMoveCommand(Guid Id, UpdateMovePayload Payload) : Activity, IRequest<MoveModel?>;

internal class UpdateMoveCommandHandler : IRequestHandler<UpdateMoveCommand, MoveModel?>
{
  private readonly IMoveQuerier _moveQuerier;
  private readonly IMoveRepository _moveRepository;

  public UpdateMoveCommandHandler(IMoveQuerier moveQuerier, IMoveRepository moveRepository)
  {
    _moveQuerier = moveQuerier;
    _moveRepository = moveRepository;
  }

  public async Task<MoveModel?> Handle(UpdateMoveCommand command, CancellationToken cancellationToken)
  {
    UpdateMovePayload payload = command.Payload;
    new UpdateMoveValidator().ValidateAndThrow(payload);

    MoveId id = new(command.Id);
    Move? move = await _moveRepository.LoadAsync(id, cancellationToken);
    if (move == null)
    {
      return null;
    }

    if (payload.Kind != null)
    {
      move.Kind = payload.Kind.Value;
    }

    Name? name = Name.TryCreate(payload.Name);
    if (name != null)
    {
      move.Name = name;
    }
    if (payload.Description != null)
    {
      move.Description = Description.TryCreate(payload.Description.Value);
    }

    if (payload.Accuracy != null)
    {
      move.Accuracy = payload.Accuracy.Value;
    }
    if (payload.Power != null)
    {
      move.Power = payload.Power.Value;
    }
    if (payload.PowerPoints.HasValue)
    {
      move.PowerPoints = payload.PowerPoints.Value;
    }

    SetStatisticChanges(payload, move);
    if (payload.Status != null)
    {
      move.Status = payload.Status.Value == null ? null : new InflictedCondition(payload.Status.Value);
    }
    SetVolatileConditions(payload, move);

    if (payload.Link != null)
    {
      move.Link = Url.TryCreate(payload.Link.Value);
    }
    if (payload.Notes != null)
    {
      move.Notes = Notes.TryCreate(payload.Notes.Value);
    }

    move.Update(command.GetUserId());
    await _moveRepository.SaveAsync(move, cancellationToken);

    return await _moveQuerier.ReadAsync(move, cancellationToken);
  }

  private static void SetStatisticChanges(UpdateMovePayload payload, Move move)
  {
    foreach (StatisticChangeModel change in payload.StatisticChanges)
    {
      move.SetStatisticChange(change.Statistic, change.Stages);
    }
  }

  private static void SetVolatileConditions(UpdateMovePayload payload, Move move)
  {
    foreach (VolatileConditionUpdate update in payload.VolatileConditions)
    {
      VolatileCondition volatileCondition = new(update.VolatileCondition);
      switch (update.Action)
      {
        case ActionKind.Add:
          move.AddVolatileCondition(volatileCondition);
          break;
        case ActionKind.Remove:
          move.RemoveVolatileCondition(volatileCondition);
          break;
        default:
          throw new ActionKindNotSupportedException(update.Action);
      }
    }
  }
}
