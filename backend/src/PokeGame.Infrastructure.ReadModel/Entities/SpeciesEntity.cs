using PokeGame.Domain;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Species;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class SpeciesEntity : Entity
  {
    public int Number { get; private set; }

    public PokemonType PrimaryType { get; private set; }
    public PokemonType? SecondaryType { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string? Category { get; private set; }
    public string? Description { get; private set; }

    public double? GenderRatio { get; private set; }
    public double? Height { get; private set; }
    public double? Weight { get; private set; }

    public int? BaseExperienceYield { get; private set; }
    public byte BaseFriendship { get; private set; }
    public byte? CatchRate { get; private set; }
    public LevelingRate LevelingRate { get; private set; }

    public string? BaseStatistics { get; private set; }
    public string? EvYield { get; private set; }

    public string? Notes { get; private set; }
    public string? Picture { get; private set; }
    public string? Reference { get; private set; }

    public List<EvolutionEntity> EvolvedFrom { get; private set; } = new();
    public List<EvolutionEntity> Evolutions { get; private set; } = new();
    public List<PokedexEntity> Pokedex { get; private set; } = new();
    public List<PokemonEntity> Pokemon { get; private set; } = new();
    public List<RegionalSpeciesEntity> RegionalSpecies { get; private set; } = new();
    public List<SpeciesAbilityEntity> SpeciesAbilities { get; private set; } = new();

    public void Add(AbilityEntity ability) => SpeciesAbilities.Add(new SpeciesAbilityEntity(this, ability));
    public EvolutionEntity Add(SpeciesEntity species)
    {
      var entity = new EvolutionEntity(this, species);
      Evolutions.Add(entity);

      return entity;
    }

    public void Synchronize(Species species)
    {
      base.Synchronize(species);

      Number = species.Number;

      PrimaryType = species.PrimaryType;
      SecondaryType = species.SecondaryType;

      Name = species.Name;
      Category = species.Category;
      Description = species.Description;

      GenderRatio = species.GenderRatio;
      Height = species.Height;
      Weight = species.Weight;

      BaseExperienceYield = species.BaseExperienceYield;
      BaseFriendship = species.BaseFriendship;
      CatchRate = species.CatchRate;
      LevelingRate = species.LevelingRate;

      IEnumerable<KeyValuePair<Statistic, byte>> baseStatistics = species.BaseStatistics.Where(x => x.Value > 0);
      BaseStatistics = baseStatistics.Any()
        ? string.Join('|', baseStatistics.Select(pair => string.Join(':', pair.Key, pair.Value)))
        : null;
      IEnumerable<KeyValuePair<Statistic, byte>> evYield = species.EvYield.Where(x => x.Value > 0);
      EvYield = evYield.Any()
        ? string.Join('|', evYield.Select(pair => string.Join(':', pair.Key, pair.Value)))
        : null;

      Notes = species.Notes;
      Picture = species.Picture;
      Reference = species.Reference;
    }
  }
}
