using FluentValidation;
using PokeGame.Domain.Items;

namespace PokeGame.Application.Items
{
  internal class ItemValidator : AbstractValidator<Item>
  {
    public ItemValidator()
    {
      When(x => x.Category == ItemCategory.PokeBall, () =>
      {
        RuleFor(x => x.DefaultModifier)
          .GreaterThan(0.0);
      }).Otherwise(() =>
      {
        RuleFor(x => x.DefaultModifier)
          .Null();
      });

      RuleFor(x => x.Price)
        .InclusiveBetween(1, 999999);

      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      RuleFor(x => x.Picture)
        .Must(ValidationRules.BeAValidUrl);

      RuleFor(x => x.Reference)
        .Must(ValidationRules.BeAValidUrl);
    }
  }
}
