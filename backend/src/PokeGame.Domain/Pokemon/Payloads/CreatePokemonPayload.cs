using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Domain.Pokemon.Payloads
{
  public class CreatePokemonPayload : SavePokemonPayload
  {
    public Guid SpeciesId { get; set; }
    public Guid AbilityId { get; set; }

    public byte Level { get; set; }
    public int? Experience { get; set; }
    public byte? Friendship { get; set; }

    public PokemonGender Gender { get; set; }
    public string Nature { get; set; } = string.Empty;

    public IEnumerable<StatisticValuePayload>? IndividualValues { get; set; }

    public ushort? CurrentHitPoints { get; set; }
  }
}
