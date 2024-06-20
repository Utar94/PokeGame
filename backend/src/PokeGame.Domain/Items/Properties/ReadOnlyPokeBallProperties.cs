using PokeGame.Contracts.Items.Properties;

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

  public ReadOnlyPokeBallProperties(double? catchRateModifier)
  {
    CatchRateModifier = catchRateModifier;
  }
}
