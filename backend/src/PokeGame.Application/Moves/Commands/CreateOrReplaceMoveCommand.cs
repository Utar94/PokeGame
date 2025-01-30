using FluentValidation;
using Logitar.EventSourcing;
using MediatR;
using PokeGame.Application.Moves.Models;
using PokeGame.Application.Moves.Validators;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

public record CreateOrReplaceMoveResult(MoveModel? Move = null, bool Created = false);

public record CreateOrReplaceMoveCommand(Guid? Id, CreateOrReplaceMovePayload Payload, long? Version) : IRequest<CreateOrReplaceMoveResult>;

internal class CreateOrReplaceMoveCommandHandler : IRequestHandler<CreateOrReplaceMoveCommand, CreateOrReplaceMoveResult>
{
  private readonly IApplicationContext _applicationContext;
  private readonly IMoveManager _moveManager;
  private readonly IMoveQuerier _moveQuerier;
  private readonly IMoveRepository _moveRepository;

  public CreateOrReplaceMoveCommandHandler(
    IApplicationContext applicationContext,
    IMoveManager moveManager,
    IMoveQuerier moveQuerier,
    IMoveRepository moveRepository)
  {
    _applicationContext = applicationContext;
    _moveManager = moveManager;
    _moveQuerier = moveQuerier;
    _moveRepository = moveRepository;
  }

  public async Task<CreateOrReplaceMoveResult> Handle(CreateOrReplaceMoveCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceMovePayload payload = command.Payload;
    new CreateOrReplaceMoveValidator().ValidateAndThrow(payload);

    MoveId? moveId = null;
    Move? move = null;
    if (command.Id.HasValue)
    {
      moveId = new(command.Id.Value);
      move = await _moveRepository.LoadAsync(moveId.Value, cancellationToken);
    }

    ActorId? actorId = _applicationContext.GetActorId();
    UniqueName uniqueName = new(payload.UniqueName);
    PowerPoints powerPoints = new(payload.PowerPoints);

    bool created = false;
    if (move == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceMoveResult();
      }

      move = new(payload.Type, payload.Category, uniqueName, powerPoints, actorId, moveId);
      created = true;
    }

    Move reference = (command.Version.HasValue
      ? await _moveRepository.LoadAsync(move.Id, command.Version.Value, cancellationToken)
      : null) ?? move;

    if (reference.UniqueName != uniqueName)
    {
      move.UniqueName = uniqueName;
    }
    DisplayName? displayName = DisplayName.TryCreate(payload.DisplayName);
    if (reference.DisplayName != displayName)
    {
      move.DisplayName = displayName;
    }
    Description? description = Description.TryCreate(payload.Description);
    if (reference.Description != description)
    {
      move.Description = description;
    }

    Accuracy? accuracy = payload.Accuracy.HasValue ? new(payload.Accuracy.Value) : null;
    if (reference.Accuracy != accuracy)
    {
      move.Accuracy = accuracy;
    }
    Power? power = payload.Power.HasValue ? new(payload.Power.Value) : null;
    if (reference.Power != power)
    {
      move.Power = power;
    }
    if (reference.PowerPoints != powerPoints)
    {
      move.PowerPoints = powerPoints;
    }

    InflictedStatus? inflictedStatus = payload.InflictedStatus == null ? null : new(payload.InflictedStatus);
    if (reference.InflictedStatus != inflictedStatus)
    {
      move.InflictedStatus = inflictedStatus;
    }
    SetStatisticChanges(payload, move, reference);
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

    move.Update(actorId);
    await _moveManager.SaveAsync(move, cancellationToken);

    MoveModel model = await _moveQuerier.ReadAsync(move, cancellationToken);
    return new CreateOrReplaceMoveResult(model, created);
  }

  private static void SetStatisticChanges(CreateOrReplaceMovePayload payload, Move move, Move reference)
  {
    Dictionary<PokemonStatistic, int> inputStatisticChanges = new(capacity: 7);
    foreach (StatisticChangeModel statisticChange in payload.StatisticChanges)
    {
      if (statisticChange.Statistic != PokemonStatistic.HP && statisticChange.Stages != 0)
      {
        inputStatisticChanges[statisticChange.Statistic] = statisticChange.Stages;
      }
    }

    foreach (KeyValuePair<PokemonStatistic, int> statisticChange in reference.StatisticChanges)
    {
      if (!inputStatisticChanges.ContainsKey(statisticChange.Key))
      {
        move.SetStatisticChange(statisticChange.Key, stages: 0);
      }
    }

    foreach (KeyValuePair<PokemonStatistic, int> statisticChange in inputStatisticChanges)
    {
      if (!reference.StatisticChanges.TryGetValue(statisticChange.Key, out int stages) || stages != statisticChange.Value)
      {
        move.SetStatisticChange(statisticChange.Key, statisticChange.Value);
      }
    }
  }

  private static void SetVolatileConditions(CreateOrReplaceMovePayload payload, Move move, Move reference)
  {
    HashSet<VolatileCondition> inputVolatileConditions = payload.VolatileConditions
      .Where(x => !string.IsNullOrWhiteSpace(x))
      .Select(value => new VolatileCondition(value))
      .ToHashSet();

    HashSet<VolatileCondition> volatileConditions = [.. move.VolatileConditions];

    foreach (VolatileCondition volatileCondition in reference.VolatileConditions)
    {
      if (!inputVolatileConditions.Contains(volatileCondition))
      {
        volatileConditions.Remove(volatileCondition);
      }
    }

    foreach (VolatileCondition volatileCondition in inputVolatileConditions)
    {
      if (!reference.HasVolatileCondition(volatileCondition))
      {
        volatileConditions.Add(volatileCondition);
      }
    }

    move.SetVolatileConditions(volatileConditions);
  }
}
