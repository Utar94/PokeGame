namespace PokeGame.Application.Pokedex.Payloads
{
  public class SeenSpeciesPayload
  {
    public IEnumerable<Guid>? TrainerIds { get; set; }
    public IEnumerable<Guid>? SpeciesIds { get; set; }
  }
}
