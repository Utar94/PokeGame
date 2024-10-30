using FluentValidation;
using MediatR;
using PokeGame.Application.Moves.Validators;
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

    // TODO(fpion): StatisticChanges
    // TODO(fpion): Status
    // TODO(fpion): VolatileConditions

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
}
