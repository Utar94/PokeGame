using PokeGame.Domain.Pokemon;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class PokemonEntity : Entity
  {
    public SpeciesEntity? Species { get; set; }
    public int SpeciesId { get; set; }
    public AbilityEntity? Ability { get; set; }
    public int AbilityId { get; set; }

    public byte Level { get; set; }
    public int Experience { get; set; }

    public PokemonGender Gender { get; set; }
    public string Nature { get; set; } = null!;
    public string? Surname { get; set; }
    public string? Description { get; set; }

    public string? IndividualValues { get; set; }
    public string? EffortValues { get; set; }
    public string? Statistics { get; set; }

    public short CurrentHitPoints { get; set; }
    public StatusCondition? StatusCondition { get; set; }

    public List<PokemonMoveEntity> Moves { get; set; } = new();
    public ItemEntity? HeldItem { get; set; }
    public int? HeldItemId { get; set; }

    public byte? MetAtLevel { get; set; }
    public string? MetLocation { get; set; }
    public DateTime? MetOn { get; set; }
    public TrainerEntity? CurrentTrainer { get; set; }
    public int? CurrentTrainerId { get; set; }
    public TrainerEntity? OriginalTrainer { get; set; }
    public int? OriginalTrainerId { get; set; }
    public byte? Position { get; set; }
    public byte? Box { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }

    public void Synchronize(Pokemon pokemon)
    {
      base.Synchronize(pokemon);

      Level = pokemon.Level;
      Experience = pokemon.Experience;

      Gender = pokemon.Gender;
      Nature = pokemon.Nature.Name;
      Surname = pokemon.Surname;
      Description = pokemon.Description;

      IEnumerable<KeyValuePair<Statistic, byte>> individualValues = pokemon.IndividualValues.Where(x => x.Value > 0);
      IndividualValues = individualValues.Any()
        ? string.Join(" | ", individualValues.Select(pair => string.Join(':', pair.Key, pair.Value)))
        : null;
      IEnumerable<KeyValuePair<Statistic, byte>> effortValues = pokemon.EffortValues.Where(x => x.Value > 0);
      EffortValues = effortValues.Any()
        ? string.Join(" | ", effortValues.Select(pair => string.Join(':', pair.Key, pair.Value)))
        : null;
      IEnumerable<KeyValuePair<Statistic, short>> statistics = pokemon.Statistics.Where(x => x.Value > 0);
      Statistics = statistics.Any()
        ? string.Join(" | ", statistics.Select(pair => string.Join(':', pair.Key, pair.Value)))
        : null;

      CurrentHitPoints = pokemon.CurrentHitPoints;
      StatusCondition = pokemon.StatusCondition;

      MetAtLevel = pokemon.History?.Level;
      MetLocation = pokemon.History?.Location;
      MetOn = pokemon.History?.MetOn;
      Position = pokemon.Position?.Position;
      Box = pokemon.Position?.Box;

      Notes = pokemon.Notes;
      Reference = pokemon.Reference;
    }
  }
}
