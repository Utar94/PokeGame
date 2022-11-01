using PokeGame.Application.Abilities.Models;
using PokeGame.Application.Models;
using PokeGame.Domain;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Application.Species.Models
{
  public class SpeciesModel : AggregateModel
  {
    public int Number { get; set; }

    public PokemonType PrimaryType { get; set; }
    public PokemonType? SecondaryType { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Description { get; set; }

    public double? GenderRatio { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }

    public int? BaseExperienceYield { get; set; }
    public byte BaseFriendship { get; set; }
    public byte? CatchRate { get; set; }
    public LevelingRate LevelingRate { get; set; }
    public byte? EggCycles { get; set; }

    public IEnumerable<StatisticValueModel> BaseStatistics { get; set; } = Enumerable.Empty<StatisticValueModel>();
    public IEnumerable<StatisticValueModel> EvYield { get; set; } = Enumerable.Empty<StatisticValueModel>();

    public string? Picture { get; set; }
    public string? PictureFemale { get; set; }
    public string? PictureShiny { get; set; }
    public string? PictureShinyFemale { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }

    public IEnumerable<AbilityModel> Abilities { get; set; } = Enumerable.Empty<AbilityModel>();
    public IEnumerable<EvolutionModel> Evolutions { get; set; } = Enumerable.Empty<EvolutionModel>();
    public IEnumerable<RegionalNumberModel> RegionalNumbers { get; set; } = Enumerable.Empty<RegionalNumberModel>();
  }
}
