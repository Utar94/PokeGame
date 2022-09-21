using PokeGame.Domain.Pokemon;

namespace PokeGame.Domain.Species
{
  public class Evolution
  {
    private Evolution(Guid speciesId, EvolutionMethod method, Guid? itemId, string? notes)
    {
      SpeciesId = speciesId;
      Method = method;
      ItemId = itemId;
      Notes = notes?.CleanTrim();
    }

    public Guid SpeciesId { get; private set; }

    public EvolutionMethod Method { get; private set; }

    public PokemonGender? Gender { get; private set; }
    public bool HighFriendship { get; private set; }
    public Guid? ItemId { get; private set; }
    public byte Level { get; private set; }
    public string? Location { get; private set; }
    public Guid? MoveId { get; private set; }
    public Region? Region { get; private set; }
    public TimeOfDay? TimeOfDay { get; private set; }

    public string? Notes { get; private set; }

    public static Evolution Item(Guid speciesId, Guid itemId, PokemonGender? gender = null, Region? region = null, string? notes = null)
      => new(speciesId, EvolutionMethod.Item, itemId, notes)
      {
        Gender = gender,
        Region = region
      };
    public static Evolution LevelUp(Guid speciesId, PokemonGender? gender = null, bool highFriendship = false, Guid? itemId = null,
      byte level = 0, string? location = null, Guid? moveId = null, Region? region = null, TimeOfDay? timeOfDay = null, string? notes = null)
      => new(speciesId, EvolutionMethod.LevelUp, itemId, notes)
      {
        Gender = gender,
        HighFriendship = highFriendship,
        Level = level,
        Location = location,
        MoveId = moveId,
        Region = region,
        TimeOfDay = timeOfDay
      };
    public static Evolution Trade(Guid speciesId, Guid? itemId = null, string? notes = null)
      => new(speciesId, EvolutionMethod.Trade, itemId, notes);
  }
}
