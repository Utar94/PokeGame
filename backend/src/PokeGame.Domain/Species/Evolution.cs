using PokeGame.Domain.Pokemon;

namespace PokeGame.Domain.Species
{
  public class Evolution
  {
    private Evolution(EvolutionMethod method, Guid? itemId, string? notes)
    {
      Method = method;
      ItemId = itemId;
      Notes = notes?.CleanTrim();
    }

    public EvolutionMethod Method { get; private set; }

    public PokemonGender? Gender { get; private set; }
    public bool HighFriendship { get; private set; }
    public Guid? ItemId { get; private set; }
    public byte Level { get; private set; }
    public string? Location { get; private set; }
    public Guid? MoveId { get; private set; }
    public Guid? RegionId { get; private set; }
    public TimeOfDay? TimeOfDay { get; private set; }

    public string? Notes { get; private set; }

    public static Evolution Item(Guid itemId, PokemonGender? gender = null, Guid? regionId = null, string? notes = null)
      => new(EvolutionMethod.Item, itemId, notes)
      {
        Gender = gender,
        RegionId = regionId
      };
    public static Evolution LevelUp(PokemonGender? gender = null, bool highFriendship = false, Guid? itemId = null,
      byte level = 0, string? location = null, Guid? moveId = null, Guid? regionId = null, TimeOfDay? timeOfDay = null, string? notes = null)
      => new(EvolutionMethod.LevelUp, itemId, notes)
      {
        Gender = gender,
        HighFriendship = highFriendship,
        Level = level,
        Location = location,
        MoveId = moveId,
        RegionId = regionId,
        TimeOfDay = timeOfDay
      };
    public static Evolution Trade(Guid? itemId = null, string? notes = null)
      => new(EvolutionMethod.Trade, itemId, notes);
  }
}
