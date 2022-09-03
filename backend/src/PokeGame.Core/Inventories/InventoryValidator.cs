using FluentValidation;

namespace PokeGame.Core.Inventories
{
  internal class InventoryValidator : AbstractValidator<Inventory>
  {
    public InventoryValidator()
    {
      RuleFor(x => x.Quantity)
        .GreaterThan(0);
    }
  }
}
