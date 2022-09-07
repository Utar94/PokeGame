using PokeGame.Domain.Pokemon.Events;
using PokeGame.Domain.Pokemon.Payloads;
using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Domain.Pokemon
{
  public class Pokemon : Aggregate
  {
    public Pokemon(CreatePokemonPayload payload, Species.Species species)
    {
      ArgumentNullException.ThrowIfNull(species);

      ApplyChange(new PokemonCreated(species.BaseStatistics, species.GenderRatio, species.LevelingRate, payload, species.Name));
    }
    private Pokemon()
    {
    }

    public Guid SpeciesId { get; private set; }
    public Guid AbilityId { get; private set; }

    public LevelingRate LevelingRate { get; private set; }
    public byte Level { get; private set; }
    public int Experience { get; private set; }

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

    public List<PokemonMove> Moves { get; private set; } = new();
    public Guid? HeldItemId { get; private set; }

    public History? History { get; private set; }
    public Guid? OriginalTrainer { get; private set; }
    public byte? Position { get; private set; }
    public byte? Box { get; private set; }

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public void Delete() => ApplyChange(new PokemonDeleted());
    public void Update(UpdatePokemonPayload payload) => ApplyChange(new PokemonUpdated(payload));

    protected virtual void Apply(PokemonCreated @event)
    {
      SpeciesId = @event.Payload.SpeciesId;
      AbilityId = @event.Payload.AbilityId;

      LevelingRate = @event.LevelingRate;
      Level = @event.Payload.Level;
      Experience = @event.Payload.Experience ?? ExperienceTable.GetTotalExperience(LevelingRate, Level);

      GenderRatio = @event.GenderRatio;
      Gender = @event.Payload.Gender;
      Nature = Nature.GetNature(@event.Payload.Nature);
      SpeciesName = @event.SpeciesName;
      Surname = @event.Payload.Surname?.CleanTrim();
      Description = @event.Payload.Description?.CleanTrim();

      BaseStatistics.Clear();
      foreach (var (statistic, value) in @event.BaseStatistics)
      {
        BaseStatistics[statistic] = value;
      }
      IndividualValues.Clear();
      if (@event.Payload.IndividualValues?.Any() == true)
      {
        foreach (StatisticValuePayload individualValue in @event.Payload.IndividualValues)
        {
          IndividualValues[individualValue.Statistic] = individualValue.Value;
        }
      }
      EffortValues.Clear();
      if (@event.Payload.EffortValues?.Any() == true)
      {
        foreach (StatisticValuePayload effortValue in @event.Payload.EffortValues)
        {
          EffortValues[effortValue.Statistic] = effortValue.Value;
        }
      }
      ComputeStatistics();

      Moves.Clear();
      if (@event.Payload.Moves?.Any() == true)
      {
        Moves.AddRange(@event.Payload.Moves.Select(move => new PokemonMove(move)));
      }
      HeldItemId = @event.Payload.HeldItemId;

      History = @event.Payload.History == null ? null : new(@event.Payload.History);
      if (@event.Payload.History != null && OriginalTrainer == null)
      {
        OriginalTrainer = @event.Payload.History.TrainerId;
      }
      Position = @event.Payload.Position;
      Box = @event.Payload.Box;

      Notes = @event.Payload.Notes?.CleanTrim();
      Reference = @event.Payload.Reference;
    }
    protected virtual void Apply(PokemonDeleted @event)
    {
      Delete(@event);
    }
    protected virtual void Apply(PokemonUpdated @event)
    {
      Description = @event.Payload.Description?.CleanTrim();

      Notes = @event.Payload.Notes?.CleanTrim();
      Reference = @event.Payload.Reference;
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
