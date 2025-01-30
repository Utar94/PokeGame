using FluentValidation;
using MediatR;
using PokeGame.Application.Moves.Models;
using PokeGame.Application.Moves.Validators;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

public record UpdateMoveCommand(Guid Id, UpdateMovePayload Payload) : IRequest<MoveModel?>;

internal class UpdateMoveCommandHandler : IRequestHandler<UpdateMoveCommand, MoveModel?>
{
  private readonly IApplicationContext _applicationContext;
  private readonly IMoveManager _moveManager;
  private readonly IMoveQuerier _moveQuerier;
  private readonly IMoveRepository _moveRepository;

  public UpdateMoveCommandHandler(
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

  public async Task<MoveModel?> Handle(UpdateMoveCommand command, CancellationToken cancellationToken)
  {
    UpdateMovePayload payload = command.Payload;
    new UpdateMoveValidator().ValidateAndThrow(payload);

    MoveId moveId = new(command.Id);
    Move? move = await _moveRepository.LoadAsync(moveId, cancellationToken);
    if (move == null)
    {
      return null;
    }

    if (!string.IsNullOrWhiteSpace(payload.UniqueName))
    {
      move.UniqueName = new UniqueName(payload.UniqueName);
    }
    if (payload.DisplayName != null)
    {
      move.DisplayName = DisplayName.TryCreate(payload.DisplayName.Value);
    }
    if (payload.Description != null)
    {
      move.Description = Description.TryCreate(payload.Description.Value);
    }

    if (payload.Accuracy != null)
    {
      Accuracy? accuracy = payload.Accuracy.Value.HasValue ? new(payload.Accuracy.Value.Value) : null;
      move.Accuracy = accuracy;
    }
    if (payload.Power != null)
    {
      Power? power = payload.Power.Value.HasValue ? new(payload.Power.Value.Value) : null;
      move.Power = power;
    }
    if (payload.PowerPoints.HasValue)
    {
      move.PowerPoints = new PowerPoints(payload.PowerPoints.Value);
    }

    if (payload.InflictedStatus != null)
    {
      InflictedStatus? inflictedStatus = payload.InflictedStatus.Value == null ? null : new(payload.InflictedStatus.Value);
      move.InflictedStatus = inflictedStatus;
    }
    foreach (StatisticChangeModel change in payload.StatisticChanges)
    {
      move.SetStatisticChange(change.Statistic, change.Stages);
    }
    // TODO(fpion): VolatileConditions

    if (payload.Link != null)
    {
      move.Link = Url.TryCreate(payload.Link.Value);
    }
    if (payload.Notes != null)
    {
      move.Notes = Notes.TryCreate(payload.Notes.Value);
    }

    move.Update(_applicationContext.GetActorId());
    await _moveManager.SaveAsync(move, cancellationToken);

    return await _moveQuerier.ReadAsync(move, cancellationToken);
  }
}
