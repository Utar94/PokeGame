namespace PokeGame.Contracts.Items.Properties;

public record PokeBallProperties : IPokeBallProperties
{
  public double? CatchRateModifier { get; set; }

  public PokeBallProperties()
  {
  }

  public PokeBallProperties(IPokeBallProperties pokeBall) : this()
  {
    CatchRateModifier = pokeBall.CatchRateModifier;
  }
}
