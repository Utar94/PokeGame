using PokeGame.Core.Abilities.Models;
using PokeGame.Core.Models;
using PokeGame.Core.Pokemon;

namespace PokeGame.Core.Species.Models
{
  public class SpeciesModel : AggregateModel
  {
    public int Number { get; set; }

    public PokemonType PrimaryType { get; set; }
    public PokemonType? SecondaryType { get; set; }

    public string Name { get; set; } = null!;
    public string? Category { get; set; }
    public string? Description { get; set; }

    public double? GenderRatio { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }

    public int? BaseExperienceYield { get; set; }
    public byte BaseFriendship { get; set; }
    public byte? CatchRate { get; set; }
    public LevelingRate LevelingRate { get; set; }

    public IEnumerable<StatisticValueModel>? BaseStatistics { get; set; }
    public IEnumerable<StatisticValueModel>? EvYield { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }

    public IEnumerable<AbilityModel> Abilities { get; set; } = null!;
  }
}
