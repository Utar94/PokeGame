using PokeGame.Domain.Pokemon;

namespace PokeGame.ReadModel.Entities
{
  internal class PokemonEntity : Entity
  {
    public SpeciesEntity? Species { get; private set; }
    public int SpeciesId { get; private set; }
    public AbilityEntity? Ability { get; private set; }
    public int AbilityId { get; private set; }

    public byte Level { get; private set; }
    public int Experience { get; private set; }
    public byte Friendship { get; private set; }
    public ushort RemainingHatchSteps { get; private set; }

    public PokemonGender Gender { get; private set; }
    public string Nature { get; private set; } = string.Empty;
    public Characteristic Characteristic { get; private set; }
    public string? Surname { get; private set; }
    public string? Description { get; private set; }

    public string? IndividualValues { get; private set; }
    public string? EffortValues { get; private set; }
    public string? Statistics { get; private set; }

    public ushort CurrentHitPoints { get; private set; }
    public StatusCondition? StatusCondition { get; private set; }

    public List<PokemonMoveEntity> Moves { get; private set; } = new();
    public ItemEntity? HeldItem { get; private set; }
    public int? HeldItemId { get; private set; }

    public ItemEntity? Ball { get; private set; }
    public int? BallId { get; private set; }
    public byte? MetAtLevel { get; private set; }
    public string? MetLocation { get; private set; }
    public DateTime? MetOn { get; private set; }
    public TrainerEntity? CurrentTrainer { get; private set; }
    public int? CurrentTrainerId { get; private set; }
    public TrainerEntity? OriginalTrainer { get; private set; }
    public int? OriginalTrainerId { get; private set; }
    public PokemonPositionEntity? Position { get; private set; }

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public void Add(MoveEntity move, PokemonMove pokemonMove)
    {
      var entity = new PokemonMoveEntity(this, move);
      entity.Synchronize(pokemonMove);
      Moves.Add(entity);
    }

    public void SetAbility(AbilityEntity ability)
    {
      Ability = ability;
      AbilityId = ability.Sid;
    }

    public void SetBall(ItemEntity? ball)
    {
      Ball = ball;
      BallId = ball?.Sid;
    }

    public void SetCurrentTrainer(TrainerEntity? currentTrainer)
    {
      CurrentTrainer = currentTrainer;
      CurrentTrainerId = currentTrainer?.Sid;
    }

    public void SetHeldItem(ItemEntity? heldItem)
    {
      HeldItem = heldItem;
      HeldItemId = heldItem?.Sid;
    }

    public void SetOriginalTrainer(TrainerEntity? originalTrainer)
    {
      OriginalTrainer = originalTrainer;
      OriginalTrainerId = originalTrainer?.Sid;
    }

    public void SetSpecies(SpeciesEntity species)
    {
      Species = species;
      SpeciesId = species.Sid;
    }

    public void Synchronize(Pokemon pokemon)
    {
      base.Synchronize(pokemon);

      Level = pokemon.Level;
      Experience = (int)pokemon.Experience;
      Friendship = pokemon.Friendship;
      RemainingHatchSteps = pokemon.RemainingHatchSteps;

      Gender = pokemon.Gender;
      Nature = pokemon.Nature.Name;
      Characteristic = pokemon.Characteristic;
      Surname = pokemon.Surname;
      Description = pokemon.Description;

      IEnumerable<KeyValuePair<Statistic, byte>> individualValues = pokemon.IndividualValues.Where(x => x.Value > 0);
      IndividualValues = individualValues.Any()
        ? string.Join("|", individualValues.Select(pair => string.Join(':', pair.Key, pair.Value)))
        : null;
      IEnumerable<KeyValuePair<Statistic, byte>> effortValues = pokemon.EffortValues.Where(x => x.Value > 0);
      EffortValues = effortValues.Any()
        ? string.Join("|", effortValues.Select(pair => string.Join(':', pair.Key, pair.Value)))
        : null;
      IEnumerable<KeyValuePair<Statistic, ushort>> statistics = pokemon.Statistics.Where(x => x.Value > 0);
      Statistics = statistics.Any()
        ? string.Join("|", statistics.Select(pair => string.Join(':', pair.Key, pair.Value)))
        : null;

      CurrentHitPoints = pokemon.CurrentHitPoints;
      StatusCondition = pokemon.StatusCondition;

      MetAtLevel = pokemon.History?.Level;
      MetLocation = pokemon.History?.Location;
      MetOn = pokemon.History?.MetOn;

      if (CurrentTrainer == null || pokemon.Position == null)
      {
        Position = null;
      }
      else
      {
        Position ??= new PokemonPositionEntity(this, CurrentTrainer);
        Position.Synchronize(pokemon.Position);
      }

      Notes = pokemon.Notes;
      Reference = pokemon.Reference;
    }
  }
}
