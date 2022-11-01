namespace PokeGame.Domain.Pokemon.Payloads
{
  public class EvolvePokemonPayload
  {
    public Guid SpeciesId { get; set; }
    public Guid AbilityId { get; set; }

    public string? Location { get; set; }
    public Guid? RegionId { get; set; }
    public TimeOfDay? TimeOfDay { get; set; }
  }
}
