using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using MediatR;
using PokeGame.Application.Moves.Validators;
using PokeGame.Contracts.Moves;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

internal class CreateMoveCommandHandler : IRequestHandler<CreateMoveCommand, Move>
{
  private readonly IMoveQuerier _moveQuerier;
  private readonly ISender _sender;

  public CreateMoveCommandHandler(IMoveQuerier moveQuerier, ISender sender)
  {
    _moveQuerier = moveQuerier;
    _sender = sender;
  }

  public async Task<Move> Handle(CreateMoveCommand command, CancellationToken cancellationToken)
  {
    IUniqueNameSettings uniqueNameSettings = MoveAggregate.UniqueNameSettings;

    CreateMovePayload payload = command.Payload;
    new CreateMoveValidator(uniqueNameSettings).ValidateAndThrow(payload);

    MoveAggregate move = new(payload.Type, payload.Category, new UniqueNameUnit(uniqueNameSettings, payload.UniqueName), command.ActorId)
    {
      DisplayName = DisplayNameUnit.TryCreate(payload.DisplayName),
      Description = DescriptionUnit.TryCreate(payload.Description),
      Accuracy = payload.Accuracy,
      Power = payload.Power,
      PowerPoints = payload.PowerPoints,
      Reference = UrlUnit.TryCreate(payload.Reference),
      Notes = NotesUnit.TryCreate(payload.Notes)
    };
    foreach (StatisticChange statisticChange in payload.StatisticChanges)
    {
      move.SetStatisticChange(statisticChange.Statistic, statisticChange.Stages);
    }
    foreach (InflictedStatusCondition inflicted in payload.StatusConditions)
    {
      StatusCondition statusCondition = new(inflicted.StatusCondition);
      move.SetStatusCondition(statusCondition, inflicted.Chance);
    }
    move.Update(command.ActorId);

    await _sender.Send(new SaveMoveCommand(move), cancellationToken);

    return await _moveQuerier.ReadAsync(move, cancellationToken);
  }
}
