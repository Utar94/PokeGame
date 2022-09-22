using PokeGame.Domain;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Species;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class EvolutionEntity
  {
    public EvolutionEntity(SpeciesEntity evolvingSpecies, SpeciesEntity evolvedSpecies)
    {
      EvolvingSpecies = evolvingSpecies ?? throw new ArgumentNullException(nameof(evolvingSpecies));
      EvolvingSpeciesId = evolvingSpecies.Sid;
      EvolvedSpecies = evolvedSpecies ?? throw new ArgumentNullException(nameof(evolvedSpecies));
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
    public Region? Region { get; private set; }
    public TimeOfDay? TimeOfDay { get; private set; }

    public string? Notes { get; private set; }

    public void Synchronize(Evolution evolution, ItemEntity? item = null, MoveEntity? move = null)
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
      Region = evolution.Region;
      TimeOfDay = evolution.TimeOfDay;

      Notes = evolution.Notes;
    }
  }
}
