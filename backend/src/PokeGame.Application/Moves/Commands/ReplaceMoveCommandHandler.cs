using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using MediatR;
using PokeGame.Application.Moves.Validators;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

internal class ReplaceMoveCommandHandler : IRequestHandler<ReplaceMoveCommand, Move?>
{
  private readonly IMoveRepository _moveRepository;
  private readonly IMoveQuerier _moveQuerier;
  private readonly ISender _sender;

  public ReplaceMoveCommandHandler(IMoveRepository moveRepository, IMoveQuerier moveQuerier, ISender sender)
  {
    _moveRepository = moveRepository;
    _moveQuerier = moveQuerier;
    _sender = sender;
  }

  public async Task<Move?> Handle(ReplaceMoveCommand command, CancellationToken cancellationToken)
  {
    IUniqueNameSettings uniqueNameSettings = MoveAggregate.UniqueNameSettings;

    MoveId id = new(command.Id);
    MoveAggregate? move = await _moveRepository.LoadAsync(id, cancellationToken);
    if (move == null)
    {
      return null;
    }
    MoveAggregate? reference = null;
    if (command.Version.HasValue)
    {
      reference = await _moveRepository.LoadAsync(move.Id, command.Version.Value, cancellationToken);
    }

    ReplaceMovePayload payload = command.Payload;
    new ReplaceMoveValidator(uniqueNameSettings, move.Category).ValidateAndThrow(payload);

    UniqueNameUnit uniqueName = new(uniqueNameSettings, payload.UniqueName);
    if (reference == null || uniqueName != reference.UniqueName)
    {
      move.UniqueName = uniqueName;
    }
    DisplayNameUnit? displayName = DisplayNameUnit.TryCreate(payload.DisplayName);
    if (reference == null || displayName != reference.DisplayName)
    {
      move.DisplayName = displayName;
    }
    DescriptionUnit? description = DescriptionUnit.TryCreate(payload.Description);
    if (reference == null || description != reference.Description)
    {
      move.Description = description;
    }

    if (reference == null || payload.Accuracy != reference.Accuracy)
    {
      move.Accuracy = payload.Accuracy;
    }
    if (reference == null || payload.Power != reference.Power)
    {
      move.Power = payload.Power;
    }
    if (reference == null || payload.PowerPoints != reference.PowerPoints)
    {
      move.PowerPoints = payload.PowerPoints;
    }

    ReplaceStatisticChanges(payload, move, reference);
    ReplaceStatusConditions(payload, move, reference);

    UrlUnit? url = UrlUnit.TryCreate(payload.Reference);
    if (reference == null || url != reference.Reference)
    {
      move.Reference = url;
    }
    NotesUnit? notes = NotesUnit.TryCreate(payload.Notes);
    if (reference == null || notes != reference.Notes)
    {
      move.Notes = notes;
    }

    move.Update(command.ActorId);

    await _sender.Send(new SaveMoveCommand(move), cancellationToken);

    return await _moveQuerier.ReadAsync(move, cancellationToken);
  }

  private static void ReplaceStatisticChanges(ReplaceMovePayload payload, MoveAggregate move, MoveAggregate? reference)
  {
    HashSet<PokemonStatistic> payloadKeys = new(capacity: payload.StatisticChanges.Count);

    IEnumerable<PokemonStatistic> referenceKeys;
    if (reference == null)
    {
      referenceKeys = move.StatisticChanges.Keys;

      foreach (StatisticChange statisticChange in payload.StatisticChanges)
      {
        payloadKeys.Add(statisticChange.Statistic);
        move.SetStatisticChange(statisticChange.Statistic, statisticChange.Stages);
      }
    }
    else
    {
      referenceKeys = reference.StatisticChanges.Keys;

      foreach (StatisticChange statisticChange in payload.StatisticChanges)
      {
        payloadKeys.Add(statisticChange.Statistic);

        if (!reference.StatisticChanges.TryGetValue(statisticChange.Statistic, out int existingStages) || statisticChange.Stages != existingStages)
        {
          move.SetStatisticChange(statisticChange.Statistic, statisticChange.Stages);
        }
      }
    }

    foreach (PokemonStatistic key in referenceKeys)
    {
      if (!payloadKeys.Contains(key))
      {
        move.RemoveStatisticChange(key);
      }
    }
  }
  private static void ReplaceStatusConditions(ReplaceMovePayload payload, MoveAggregate move, MoveAggregate? reference)
  {
    HashSet<StatusCondition> payloadKeys = new(capacity: payload.StatusConditions.Count);

    IEnumerable<StatusCondition> referenceKeys;
    if (reference == null)
    {
      referenceKeys = move.StatusConditions.Keys;

      foreach (InflictedStatusCondition statusCondition in payload.StatusConditions)
      {
        StatusCondition key = new(statusCondition.StatusCondition);
        payloadKeys.Add(key);
        move.SetStatusCondition(key, statusCondition.Chance);
      }
    }
    else
    {
      referenceKeys = reference.StatusConditions.Keys;

      foreach (InflictedStatusCondition statusCondition in payload.StatusConditions)
      {
        StatusCondition key = new(statusCondition.StatusCondition);
        payloadKeys.Add(key);

        if (!reference.StatusConditions.TryGetValue(key, out int existingChance) || statusCondition.Chance != existingChance)
        {
          move.SetStatusCondition(key, statusCondition.Chance);
        }
      }
    }

    foreach (StatusCondition statusCondition in referenceKeys)
    {
      if (!payloadKeys.Contains(statusCondition))
      {
        move.RemoveStatusCondition(statusCondition);
      }
    }
  }
}
