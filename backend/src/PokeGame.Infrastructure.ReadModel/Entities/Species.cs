using PokeGame.Domain;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class Species : Entity
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

    public string? BaseStatistics { get; set; }
    public string? EvYield { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }

    public List<Ability> Abilities { get; set; } = new();

    public void Synchronize(Domain.Species.Species species)
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

      BaseStatistics = species.BaseStatistics.Any()
        ? string.Join('|', species.BaseStatistics.Where(x => x.Value != 0).Select(pair => string.Join(':', pair.Key, pair.Value)))
        : null;
      EvYield = species.EvYield.Any()
        ? string.Join('|', species.EvYield.Where(x => x.Value != 0).Select(pair => string.Join(':', pair.Key, pair.Value)))
        : null;

      Notes = species.Notes;
      Reference = species.Reference;
    }
  }
}
