using PokeGame.Domain.Pokemon.Events;
using PokeGame.Domain.Pokemon.Payloads;
using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Domain.Pokemon
{
  public class Pokemon : Aggregate
  {
    public Pokemon(CreatePokemonPayload payload, Species.Species species)
    {
      ApplyChange(PokemonCreated.Create(payload, species));
    }
    private Pokemon()
    {
    }

    public Guid SpeciesId { get; private set; }
    public Guid AbilityId { get; private set; }

    public byte BaseFriendship { get; private set; }
    public LevelingRate LevelingRate { get; private set; }
    public byte Level { get; private set; }
    public int Experience { get; private set; }
    public byte Friendship { get; private set; }

    public double? GenderRatio { get; private set; }
    public PokemonGender Gender { get; private set; }
    public Nature Nature { get; private set; } = null!;
    public string SpeciesName { get; private set; } = null!;
    public string? Surname { get; private set; }
    public string? Description { get; private set; }

    public Dictionary<Statistic, byte> BaseStatistics { get; private set; } = new();
    public Dictionary<Statistic, byte> IndividualValues { get; private set; } = new();
    public Dictionary<Statistic, byte> EffortValues { get; private set; } = new();
    public Dictionary<Statistic, short> Statistics { get; private set; } = new();

    public short MaximumHitPoints => Statistics.TryGetValue(Statistic.HP, out short maximumHitPoints) ? maximumHitPoints : (short)0;
    public short CurrentHitPoints { get; private set; }
    public StatusCondition? StatusCondition { get; private set; }

    public List<PokemonMove> Moves { get; private set; } = new();
    public Guid? HeldItemId { get; private set; }

    public History? History { get; private set; }
    public Guid? OriginalTrainerId { get; private set; }
    public byte? Position { get; private set; }
    public byte? Box { get; private set; }

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public void Delete() => ApplyChange(new PokemonDeleted());
    public void Update(UpdatePokemonPayload payload) => ApplyChange(new PokemonUpdated(payload));

    public void Heal(short restoreHitPoints, bool removeCondition) => ApplyChange(new PokemonHealed(restoreHitPoints, removeCondition));

    protected virtual void Apply(PokemonCreated @event)
    {
      CreatePokemonPayload payload = @event.Payload;

      SpeciesId = payload.SpeciesId;
      AbilityId = payload.AbilityId;

      BaseFriendship = @event.BaseFriendship;
      LevelingRate = @event.LevelingRate;
      Level = payload.Level;
      Experience = payload.Experience ?? ExperienceTable.GetTotalExperience(LevelingRate, Level);
      Friendship = payload.Friendship ?? BaseFriendship;

      GenderRatio = @event.GenderRatio;
      Gender = payload.Gender;
      Nature = Nature.GetNature(payload.Nature, nameof(payload.Nature));
      SpeciesName = @event.SpeciesName;
      Surname = payload.Surname?.CleanTrim();
      Description = payload.Description?.CleanTrim();

      BaseStatistics.Clear();
      foreach (var (statistic, value) in @event.BaseStatistics)
      {
        BaseStatistics[statistic] = value;
      }
      IndividualValues.Clear();
      if (payload.IndividualValues?.Any() == true)
      {
        foreach (StatisticValuePayload individualValue in payload.IndividualValues)
        {
          IndividualValues[individualValue.Statistic] = individualValue.Value;
        }
      }
      EffortValues.Clear();
      if (payload.EffortValues?.Any() == true)
      {
        foreach (StatisticValuePayload effortValue in payload.EffortValues)
        {
          EffortValues[effortValue.Statistic] = effortValue.Value;
        }
      }
      ComputeStatistics();

      CurrentHitPoints = payload.CurrentHitPoints
        ?? (Statistics.TryGetValue(Statistic.HP, out short totalHitPoints) ? totalHitPoints : (short)0);
      StatusCondition = payload.StatusCondition;

      Moves.Clear();
      if (payload.Moves?.Any() == true)
      {
        Moves.AddRange(payload.Moves.Select(move => new PokemonMove(move)));
      }
      HeldItemId = payload.HeldItemId;

      History = payload.History == null ? null : new(payload.History);
      if (payload.History != null && OriginalTrainerId == null)
      {
        OriginalTrainerId = payload.History.TrainerId;
      }
      Position = payload.Position;
      Box = payload.Box;

      Notes = payload.Notes?.CleanTrim();
      Reference = payload.Reference;
    }
    protected virtual void Apply(PokemonDeleted @event)
    {
      Delete(@event);
    }
    protected virtual void Apply(PokemonHealed @event)
    {
      CurrentHitPoints += @event.RestoreHitPoints;
      if (CurrentHitPoints > MaximumHitPoints)
      {
        CurrentHitPoints = MaximumHitPoints;
      }

      if (@event.RemoveCondition)
      {
        StatusCondition = null;
      }
    }
    protected virtual void Apply(PokemonUpdated @event)
    {
      UpdatePokemonPayload payload = @event.Payload;

      Description = payload.Description?.CleanTrim();

      Notes = payload.Notes?.CleanTrim();
      Reference = payload.Reference;
    }

    private void ComputeStatistics()
    {
      Statistics[Statistic.HP] = this.CalculateStatistic(Statistic.HP);
      Statistics[Statistic.Attack] = this.CalculateStatistic(Statistic.Attack);
      Statistics[Statistic.Defense] = this.CalculateStatistic(Statistic.Defense);
      Statistics[Statistic.SpecialAttack] = this.CalculateStatistic(Statistic.SpecialAttack);
      Statistics[Statistic.SpecialDefense] = this.CalculateStatistic(Statistic.SpecialDefense);
      Statistics[Statistic.Speed] = this.CalculateStatistic(Statistic.Speed);
    }

    public override string ToString() => $"{Surname ?? SpeciesName} | {base.ToString()}";
  }
}

/*
 * TODO(fpion):
 * - Evolution (SpeciesId change => AbilityId, BaseStatistics, LevelingRate, SpeciesName can change), Moves, Statistics
 * - Gain experience & effort values
 * - Level-Up, Moves, Statistics
 * - Change Surname
 * - Change Held Item (add, remove, update)
 * - Change History => change Current Trainer
 * - Move (Box & Position)
 */
