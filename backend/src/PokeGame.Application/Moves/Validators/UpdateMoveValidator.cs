using FluentValidation;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Moves.Validators;

internal class UpdateMoveValidator : AbstractValidator<UpdateMovePayload>
{
  public UpdateMoveValidator()
  {
    When(x => !string.IsNullOrWhiteSpace(x.Name), () => RuleFor(x => x.Name!).Name());
    When(x => !string.IsNullOrWhiteSpace(x.Description?.Value), () => RuleFor(x => x.Description!.Value!).Description());

    // TODO(fpion): Accuracy
    // TODO(fpion): Power
    // TODO(fpion): PowerPoints

    When(x => !string.IsNullOrWhiteSpace(x.Link?.Value), () => RuleFor(x => x.Link!.Value!).Url());
    When(x => !string.IsNullOrWhiteSpace(x.Notes?.Value), () => RuleFor(x => x.Notes!.Value!).Notes());
  }
}
