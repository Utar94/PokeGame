using FluentValidation;

namespace PokeGame.Domain.Validators;

public class NotesValidator : AbstractValidator<string>
{
  public NotesValidator()
  {
    RuleFor(x => x).NotEmpty();
  }
}
