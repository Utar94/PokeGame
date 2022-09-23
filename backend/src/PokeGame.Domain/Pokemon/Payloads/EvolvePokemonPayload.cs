namespace PokeGame.Domain.Pokemon.Payloads
{
  public class EvolvePokemonPayload
  {
    public Guid SpeciesId { get; set; }
    public Guid AbilityId { get; set; }

    public string? Location { get; set; }
    public Region? Region { get; set; }
    public TimeOfDay? TimeOfDay { get; set; }
  }
}
