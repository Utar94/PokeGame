using FluentValidation;
using MediatR;
using PokeGame.Application.Moves.Validators;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

public record CreateOrReplaceMoveResult(MoveModel? Move = null, bool Created = false);

public record CreateOrReplaceMoveCommand(Guid? Id, CreateOrReplaceMovePayload Payload, long? Version) : Activity, IRequest<CreateOrReplaceMoveResult>;

internal class CreateOrReplaceMoveCommandHandler : IRequestHandler<CreateOrReplaceMoveCommand, CreateOrReplaceMoveResult>
{
  private readonly IMoveQuerier _moveQuerier;
  private readonly IMoveRepository _moveRepository;

  public CreateOrReplaceMoveCommandHandler(IMoveQuerier moveQuerier, IMoveRepository moveRepository)
  {
    _moveQuerier = moveQuerier;
    _moveRepository = moveRepository;
  }

  public async Task<CreateOrReplaceMoveResult> Handle(CreateOrReplaceMoveCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceMovePayload payload = command.Payload;
    new CreateOrReplaceMoveValidator().ValidateAndThrow(payload);

    MoveId? id = command.Id.HasValue ? new(command.Id.Value) : null;
    Move? move = null;
    if (id.HasValue)
    {
      move = await _moveRepository.LoadAsync(id.Value, cancellationToken);
    }

    UserId userId = command.GetUserId();
    bool created = false;
    if (move == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceMoveResult();
      }

      move = new Move(payload.Type, payload.Category, new Name(payload.Name), userId, id);
      created = true;
    }

    Move reference = (command.Version.HasValue
      ? await _moveRepository.LoadAsync(move.Id, command.Version.Value, cancellationToken)
      : null) ?? move;

    Name name = new(payload.Name);
    if (reference.Name != name)
    {
      move.Name = name;
    }
    Description? description = Description.TryCreate(payload.Description);
    if (reference.Description != description)
    {
      move.Description = description;
    }

    if (reference.Accuracy != payload.Accuracy)
    {
      move.Accuracy = payload.Accuracy;
    }
    if (reference.Power != payload.Power)
    {
      move.Power = payload.Power;
    }
    if (reference.PowerPoints != payload.PowerPoints)
    {
      move.PowerPoints = payload.PowerPoints;
    }

    SetStatisticChanges(payload, move, reference);
    InflictedCondition? status = payload.Status == null ? null : new(payload.Status);
    if (reference.Status != status)
    {
      move.Status = status;
    }
    SetVolatileConditions(payload, move, reference);

    Url? link = Url.TryCreate(payload.Link);
    if (reference.Link != link)
    {
      move.Link = link;
    }
    Notes? notes = Notes.TryCreate(payload.Notes);
    if (reference.Notes != notes)
    {
      move.Notes = notes;
    }

    move.Update(userId);
    await _moveRepository.SaveAsync(move, cancellationToken);

    MoveModel model = await _moveQuerier.ReadAsync(move, cancellationToken);
    return new CreateOrReplaceMoveResult(model, created);
  }

  private static void SetStatisticChanges(CreateOrReplaceMovePayload payload, Move move, Move reference)
  {
    HashSet<BattleStatistic> statistics = new(capacity: payload.StatisticChanges.Count);
    foreach (StatisticChangeModel change in payload.StatisticChanges)
    {
      statistics.Add(change.Statistic);
      if (!reference.StatisticChanges.TryGetValue(change.Statistic, out int existingStages) || existingStages != change.Stages)
      {
        move.SetStatisticChange(change.Statistic, change.Stages);
      }
    }
    foreach (BattleStatistic statistic in reference.StatisticChanges.Keys)
    {
      if (!statistics.Contains(statistic))
      {
        move.SetStatisticChange(statistic, stages: 0);
      }
    }
  }

  private static void SetVolatileConditions(CreateOrReplaceMovePayload payload, Move move, Move reference)
  {
    HashSet<VolatileCondition> volatileConditions = new(capacity: payload.VolatileConditions.Count);
    foreach (string value in payload.VolatileConditions)
    {
      VolatileCondition volatileCondition = new(value);
      volatileConditions.Add(volatileCondition);
      if (!reference.VolatileConditions.Contains(volatileCondition))
      {
        move.AddVolatileCondition(volatileCondition);
      }
    }
    foreach (VolatileCondition volatileCondition in reference.VolatileConditions)
    {
      if (!volatileConditions.Contains(volatileCondition))
      {
        move.RemoveVolatileCondition(volatileCondition);
      }
    }
  }
}
