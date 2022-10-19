using PokeGame.Domain.Pokemon;

namespace PokeGame.Domain.Species.Payloads
{
  public class SaveEvolutionPayload
  {
    public EvolutionMethod Method { get; set; }

    public PokemonGender? Gender { get; set; }
    public bool HighFriendship { get; set; }
    public Guid? ItemId { get; set; }
    public byte Level { get; set; }
    public string? Location { get; set; }
    public Guid? MoveId { get; set; }
    public Region? Region { get; set; }
    public TimeOfDay? TimeOfDay { get; set; }

    public string? Notes { get; set; }
  }
}
