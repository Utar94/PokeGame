using PokeGame.Domain;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Species;

namespace PokeGame.ReadModel.Entities
{
  internal class EvolutionEntity
  {
    public EvolutionEntity(SpeciesEntity evolvingSpecies, SpeciesEntity evolvedSpecies)
    {
      EvolvingSpecies = evolvingSpecies;
      EvolvingSpeciesId = evolvingSpecies.Sid;
      EvolvedSpecies = evolvedSpecies;
      EvolvedSpeciesId = evolvedSpecies.Sid;
    }
    private EvolutionEntity()
    {
    }

    public SpeciesEntity? EvolvingSpecies { get; private set; }
    public int EvolvingSpeciesId { get; private set; }
    public SpeciesEntity? EvolvedSpecies { get; private set; }
    public int EvolvedSpeciesId { get; private set; }

    public EvolutionMethod Method { get; private set; }

    public PokemonGender? Gender { get; private set; }
    public bool HighFriendship { get; private set; }
    public ItemEntity? Item { get; private set; }
    public int? ItemId { get; private set; }
    public byte Level { get; private set; }
    public string? Location { get; private set; }
    public MoveEntity? Move { get; private set; }
    public int? MoveId { get; private set; }
    public RegionEntity? Region { get; private set; }
    public int? RegionId { get; private set; }
    public TimeOfDay? TimeOfDay { get; private set; }

    public string? Notes { get; private set; }

    public void Synchronize(Evolution evolution, ItemEntity? item = null, MoveEntity? move = null, RegionEntity? region = null)
    {
      Method = evolution.Method;

      Gender = evolution.Gender;
      HighFriendship = evolution.HighFriendship;
      Item = item;
      ItemId = item?.Sid;
      Level = evolution.Level;
      Location = evolution.Location;
      Move = move;
      MoveId = move?.Sid;
      Region = region;
      RegionId = region?.Sid;
      TimeOfDay = evolution.TimeOfDay;

      Notes = evolution.Notes;
    }
  }
}
