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
    // TODO(fpion): StatisticChanges
    // TODO(fpion): VolatileConditions

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
}
