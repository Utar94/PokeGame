namespace PokeGame.Domain.Pokemon.Payloads
{
  public class UpdatePokemonPayload
  {
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
