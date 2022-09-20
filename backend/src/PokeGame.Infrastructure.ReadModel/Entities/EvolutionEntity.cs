using PokeGame.Domain;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Species;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class EvolutionEntity
  {
    public EvolutionEntity(SpeciesEntity evolvingSpecies, SpeciesEntity evolvedSpecies, ItemEntity? item = null, MoveEntity? move = null)
    {
      EvolvingSpecies = evolvingSpecies ?? throw new ArgumentNullException(nameof(evolvingSpecies));
      EvolvingSpeciesId = evolvingSpecies.Sid;
      EvolvedSpecies = evolvedSpecies ?? throw new ArgumentNullException(nameof(evolvedSpecies));
      EvolvedSpeciesId = evolvedSpecies.Sid;
      Item = item;
      ItemId = item?.Sid;
      Move = move;
      MoveId = move?.Sid;
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
    public Region? Region { get; private set; }
    public TimeOfDay? TimeOfDay { get; private set; }

    public string? Notes { get; private set; }

    public void Synchronize(Evolution evolution)
    {
      Method = evolution.Method;

      Gender = evolution.Gender;
      HighFriendship = evolution.HighFriendship;
      Level = evolution.Level;
      Location = evolution.Location;
      Region = evolution.Region;
      TimeOfDay = evolution.TimeOfDay;

      Notes = evolution.Notes;
    }
  }
}
