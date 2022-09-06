namespace PokeGame.Domain.Species.Payloads
{
  public class CreateSpeciesPayload : SaveSpeciesPayload
  {
    public int Number { get; set; }

    public PokemonType PrimaryType { get; set; }
    public PokemonType? SecondaryType { get; set; }
  }
}
