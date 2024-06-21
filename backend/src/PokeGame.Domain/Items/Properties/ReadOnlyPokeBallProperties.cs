using FluentValidation;
using PokeGame.Contracts.Items.Properties;
using PokeGame.Domain.Items.Validators;

namespace PokeGame.Domain.Items.Properties;

public record ReadOnlyPokeBallProperties : ItemProperties, IPokeBallProperties
{
  public double? CatchRateModifier { get; }

  public ReadOnlyPokeBallProperties() : this(catchRateModifier: null)
  {
  }

  public ReadOnlyPokeBallProperties(IPokeBallProperties pokeBall) : this(pokeBall.CatchRateModifier)
  {
  }

  [JsonConstructor]
  public ReadOnlyPokeBallProperties(double? catchRateModifier)
  {
    CatchRateModifier = catchRateModifier;
    new PokeBallPropertiesValidator().ValidateAndThrow(this);
  }
}
