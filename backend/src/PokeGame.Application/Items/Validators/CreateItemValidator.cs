using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using PokeGame.Contracts.Items;
using PokeGame.Domain.Items.Validators;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Items.Validators;

internal class CreateItemValidator : AbstractValidator<CreateItemPayload>
{
  public CreateItemValidator(IUniqueNameSettings uniqueNameSettings, ItemCategory category)
  {
    RuleFor(x => x.Category).IsInEnum();
    When(x => x.Price.HasValue, () => RuleFor(x => x.Price!.Value).SetValidator(new PriceValidator()));

    RuleFor(x => x.UniqueName).SetValidator(new UniqueNameValidator(uniqueNameSettings));
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).SetValidator(new DisplayNameValidator()));
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).SetValidator(new DescriptionValidator()));
    When(x => !string.IsNullOrWhiteSpace(x.Picture), () => RuleFor(x => x.Picture!).SetValidator(new UrlValidator()));

    switch (category)
    {
      case ItemCategory.Medicine:
        When(x => x.Medicine != null, () => RuleFor(x => x.Medicine!).SetValidator(new MedicinePropertiesValidator()))
          .Otherwise(() => RuleFor(x => x.Medicine).NotNull());
        RuleFor(x => x.PokeBall).Null();
        break;
      case ItemCategory.PokeBall:
        When(x => x.PokeBall != null, () => RuleFor(x => x.PokeBall!).SetValidator(new PokeBallPropertiesValidator()))
          .Otherwise(() => RuleFor(x => x.PokeBall).NotNull());
        RuleFor(x => x.Medicine).Null();
        break;
      default:
        RuleFor(x => x.Medicine).Null();
        RuleFor(x => x.PokeBall).Null();
        break;
    }

    When(x => !string.IsNullOrWhiteSpace(x.Reference), () => RuleFor(x => x.Reference!).SetValidator(new UrlValidator()));
    When(x => !string.IsNullOrWhiteSpace(x.Notes), () => RuleFor(x => x.Notes!).SetValidator(new NotesValidator()));
  }
}
