﻿using PokeGame.Domain.Items;
using PokeGame.Domain.Moves;
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

    public LevelingRate LevelingRate { get; private set; }
    public byte Level { get; private set; }
    public uint Experience { get; private set; }
    public byte Friendship { get; private set; }
    public ushort RemainingHatchSteps { get; private set; }
    public bool HasHighFriendshid => Friendship >= 220;

    public double? GenderRatio { get; private set; }
    public PokemonGender Gender { get; private set; }
    public bool IsShiny { get; private set; }
    public Nature Nature { get; private set; } = null!;
    public Characteristic Characteristic { get; private set; }
    public string SpeciesName { get; private set; } = string.Empty;
    public string? Surname { get; private set; }
    public string? Description { get; private set; }

    public Dictionary<Statistic, byte> BaseStatistics { get; private set; } = new();
    public Dictionary<Statistic, byte> IndividualValues { get; private set; } = new();
    public Dictionary<Statistic, byte> EffortValues { get; private set; } = new();
    public int RemainingEffortValues => 510 - EffortValues.Sum(x => x.Value);
    public Dictionary<Statistic, ushort> Statistics { get; private set; } = new();
    public ushort MaximumHitPoints => Statistics.TryGetValue(Statistic.HP, out ushort maximumHitPoints) ? maximumHitPoints : (ushort)0;
    public ushort Attack => Statistics.TryGetValue(Statistic.Attack, out ushort attack) ? attack : (ushort)0;
    public ushort Defense => Statistics.TryGetValue(Statistic.Defense, out ushort defense) ? defense : (ushort)0;
    public ushort SpecialAttack => Statistics.TryGetValue(Statistic.SpecialAttack, out ushort specialAttack) ? specialAttack : (ushort)0;
    public ushort SpecialDefense => Statistics.TryGetValue(Statistic.SpecialDefense, out ushort specialDefense) ? specialDefense : (ushort)0;
    public ushort Speed => Statistics.TryGetValue(Statistic.Speed, out ushort speed) ? speed : (ushort)0;

    public ushort CurrentHitPoints { get; private set; }
    public StatusCondition? StatusCondition { get; private set; }

    public List<PokemonMove> Moves { get; private set; } = new();
    public Guid? HeldItemId { get; private set; }

    public History? History { get; private set; }
    public Guid? OriginalTrainerId { get; private set; }
    public PokemonPosition? Position { get; private set; }
    public bool IsTraded => History?.TrainerId != OriginalTrainerId;

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public void Delete() => ApplyChange(new PokemonDeleted());
    public void Update(UpdatePokemonPayload payload) => ApplyChange(new PokemonUpdated(payload));

    public void Catch(Guid ballId, string location, Guid trainerId, byte position, byte? box = null, byte? friendship = null, string? surname = null)
    {
      if (OriginalTrainerId.HasValue || History != null)
      {
        throw new CannotCatchTrainerPokemonException(this);
      }

      ApplyChange(new PokemonCaught(ballId, location, trainerId, position, box, friendship, surname));
    }
    public void Evolve(EvolvePokemonPayload payload, Species.Species species, bool removeHeldItem = false) => ApplyChange(PokemonEvolved.Create(payload, species, removeHeldItem));
    public void GainedExperience(ExperienceGainPayload payload, bool hasBeenCaughtWithLuxuryBall, bool isHoldingSootheBell)
      => ApplyChange(new PokemonGainedExperience(payload, hasBeenCaughtWithLuxuryBall, isHoldingSootheBell));
    public void Heal(HealPokemonPayload payload, IEnumerable<Move>? moves = null) => ApplyChange(new PokemonHealed(payload, moves?.ToDictionary(x => x.Id, x => x.PowerPoints)));
    public void HoldItem(Item? item) => ApplyChange(new PokemonHeldItem(item?.Id));
    public void Move(PokemonPosition? position) => ApplyChange(new PokemonMoved(position?.Position, position?.Box));
    public void UpdateCondition(UpdatePokemonConditionPayload payload) => ApplyChange(new UpdatedPokemonCondition(payload));
    public void UseMove(Move move, UsePokemonMovePayload payload)
    {
      PokemonMove pokemonMove = Moves.SingleOrDefault(x => x.MoveId == move.Id)
        ?? throw new PokemonMoveNotFoundException(this, move);

      if (pokemonMove.RemainingPowerPoints == 0)
      {
        throw new NoRemainingPowerPointException(this, move);
      }

      ApplyChange(new PokemonUsedMove(move.Id, payload));
    }
    public void WalkEgg(ushort steps)
    {
      if (RemainingHatchSteps == 0)
      {
        throw new PokemonEggAlreadyHatchedException(this);
      }

      ApplyChange(new PokemonEggWalked(steps));
    }
    public void Wound(ushort damage, byte attackerLevel, StatusCondition? statusCondition = null)
    {
      if (CurrentHitPoints == 0)
      {
        throw new CannotWoundFaintedPokemonException(this);
      }

      ApplyChange(new PokemonWounded(damage, statusCondition, attackerLevel));
    }

    protected virtual void Apply(PokemonCaught @event)
    {
      if (@event.Friendship.HasValue)
      {
        Friendship = Math.Max(Friendship, @event.Friendship.Value);
      }
      Surname = @event.Surname?.CleanTrim();

      SetHistory(new HistoryPayload
      {
        BallId = @event.BallId,
        Level = Level,
        Location = @event.Location,
        MetOn = DateTime.UtcNow,
        TrainerId = @event.TrainerId
      });

      Position = new(@event.Position, @event.Box);
    }
    protected virtual void Apply(PokemonCreated @event)
    {
      CreatePokemonPayload payload = @event.Payload;
      Apply(payload);

      SpeciesId = payload.SpeciesId;
      AbilityId = payload.AbilityId;

      LevelingRate = @event.LevelingRate;
      Level = payload.Level;
      Experience = payload.Experience ?? ExperienceTable.GetTotalExperience(LevelingRate, Level);
      Friendship = payload.Friendship ?? @event.BaseFriendship;
      RemainingHatchSteps = payload.RemainingHatchSteps;

      GenderRatio = @event.GenderRatio;
      Gender = payload.Gender;
      IsShiny = payload.IsShiny;
      Nature = Nature.GetNature(payload.Nature, nameof(payload.Nature));
      Characteristic = payload.Characteristic;
      SpeciesName = @event.SpeciesName;

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
      ComputeStatistics();

      CurrentHitPoints = payload.CurrentHitPoints
        ?? (Statistics.TryGetValue(Statistic.HP, out ushort totalHitPoints) ? totalHitPoints : (ushort)0);

      SetMoves(payload);
    }
    protected virtual void Apply(PokemonDeleted @event)
    {
      Delete(@event);
    }
    protected virtual void Apply(PokemonEggWalked @event)
    {
      RemainingHatchSteps = (RemainingHatchSteps < @event.Steps)
        ? (ushort)0
        : (ushort)(RemainingHatchSteps - @event.Steps);
    }
    protected virtual void Apply(PokemonEvolved @event)
    {
      EvolvePokemonPayload payload = @event.Payload;

      SpeciesId = payload.SpeciesId;
      AbilityId = payload.AbilityId;

      LevelingRate = @event.LevelingRate;

      GenderRatio = @event.GenderRatio;
      SpeciesName = @event.SpeciesName;

      BaseStatistics.Clear();
      foreach (var (statistic, value) in @event.BaseStatistics)
      {
        BaseStatistics[statistic] = value;
      }

      ushort hitPointsLost = (ushort)(MaximumHitPoints - CurrentHitPoints);
      ComputeStatistics();
      CurrentHitPoints = (ushort)(MaximumHitPoints - hitPointsLost);

      if (@event.RemoveHeldItem)
      {
        HeldItemId = null;
      }
    }
    protected virtual void Apply(PokemonGainedExperience @event)
    {
      ExperienceGainPayload payload = @event.Payload;

      uint maximumExperience = ExperienceTable.GetTotalExperience(LevelingRate, 100);
      uint experience = Experience + payload.Experience;
      Experience = (experience < Experience || experience > maximumExperience)
        ? maximumExperience
        : experience;

      ushort hitPointsLost = (ushort)(MaximumHitPoints - CurrentHitPoints);
      byte previousLevel = Level;
      for (byte level = (byte)(Level + 1); level <= 100; level++)
      {
        if (Experience < ExperienceTable.GetTotalExperience(LevelingRate, level))
        {
          break;
        }

        Level = level;

        byte gain = (byte)(Friendship < 100 ? 5 : (Friendship < 200 ? 3 : 2));

        if (@event.HasBeenCaughtWithLuxuryBall)
        {
          gain *= 2;
        }
        if (@event.IsHoldingSootheBell)
        {
          gain = (byte)Math.Floor(gain * 1.5);
        }

        Friendship += (byte)Math.Min(gain, byte.MaxValue - Friendship);
      }
      if (Level > previousLevel)
      {
        ComputeStatistics();
        CurrentHitPoints = (ushort)(MaximumHitPoints - hitPointsLost);
      }

      byte friendship = (byte)(Friendship + payload.Friendship);
      Friendship = friendship < Friendship ? byte.MaxValue : Friendship;

      if (payload.EffortValues != null && RemainingEffortValues > 0)
      {
        foreach (StatisticValuePayload effortValue in payload.EffortValues)
        {
          byte gain = (byte)Math.Min(effortValue.Value, RemainingEffortValues);

          if (EffortValues.TryGetValue(effortValue.Statistic, out byte value))
          {
            value += gain;
            EffortValues[effortValue.Statistic] = value < EffortValues[effortValue.Statistic]
              ? byte.MaxValue
              : value;
          }
          else
          {
            EffortValues.Add(effortValue.Statistic, gain);
          }

          if (RemainingEffortValues < 1)
          {
            break;
          }
        }
      }
    }
    protected virtual void Apply(PokemonHealed @event)
    {
      HealPokemonPayload payload = @event.Payload;

      if (payload.RestoreAllPowerPoints && @event.MovePowerPoints?.Any() == true)
      {
        foreach (PokemonMove move in Moves)
        {
          if (@event.MovePowerPoints.TryGetValue(move.MoveId, out byte powerPoints))
          {
            move.Restore(powerPoints);
          }
        }
      }

      ushort currentHitPoints = (ushort)(CurrentHitPoints + payload.RestoreHitPoints);
      CurrentHitPoints = (currentHitPoints < CurrentHitPoints || currentHitPoints > MaximumHitPoints)
        ? MaximumHitPoints
        : currentHitPoints;

      if (payload.RemoveAllConditions || payload.StatusCondition == StatusCondition)
      {
        StatusCondition = null;
      }
    }
    protected virtual void Apply(PokemonHeldItem @event)
    {
      HeldItemId = @event.ItemId;
    }
    protected virtual void Apply(PokemonMoved @event)
    {
      Position = @event.Position.HasValue ? new(@event.Position.Value, @event.Box) : null;
    }
    protected virtual void Apply(PokemonUpdated @event)
    {
      UpdatePokemonPayload payload = @event.Payload;
      Apply(payload);

      Friendship = payload.Friendship;

      CurrentHitPoints = payload.CurrentHitPoints;

      OriginalTrainerId = payload.OriginalTrainerId;

      SetMoves(@event.Payload);
    }
    protected virtual void Apply(PokemonUsedMove @event)
    {
      Moves.Single(x => x.MoveId == @event.MoveId).Use();
    }
    protected virtual void Apply(PokemonWounded @event)
    {
      if (CurrentHitPoints > @event.Damage)
      {
        CurrentHitPoints -= @event.Damage;

        if (@event.StatusCondition.HasValue)
        {
          StatusCondition = @event.StatusCondition.Value;
        }
      }
      else
      {
        CurrentHitPoints = 0;
        StatusCondition = null;

        if (@event.AttackerLevel.HasValue)
        {
          if (Level + 30 <= @event.AttackerLevel.Value)
          {
            if (Friendship >= 200)
            {
              Friendship -= 10;
            }
            else if (Friendship >= 5)
            {
              Friendship -= 5;
            }
            else
            {
              Friendship = 0;
            }
          }
          else if (Friendship >= 1)
          {
            Friendship -= 1;
          }
          else
          {
            Friendship = 0;
          }
        }
      }
    }
    protected virtual void Apply(UpdatedPokemonCondition @event)
    {
      CurrentHitPoints = @event.Payload.CurrentHitPoints;
      StatusCondition = @event.Payload.StatusCondition;
    }

    private void Apply(SavePokemonPayload payload)
    {
      Surname = payload.Surname?.CleanTrim();
      Description = payload.Description?.CleanTrim();

      Notes = payload.Notes?.CleanTrim();
      Reference = payload.Reference;

      EffortValues.Clear();
      if (payload.EffortValues?.Any() == true)
      {
        foreach (StatisticValuePayload effortValue in payload.EffortValues)
        {
          EffortValues[effortValue.Statistic] = effortValue.Value;
        }
      }

      StatusCondition = payload.StatusCondition;

      HeldItemId = payload.HeldItemId;

      SetHistory(payload.History);
      Position = payload.Position.HasValue ? new(payload.Position.Value, payload.Box) : null;
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

    private void SetHistory(HistoryPayload? payload)
    {
      History = payload == null ? null : new(payload);
      if (payload != null && OriginalTrainerId == null)
      {
        OriginalTrainerId = payload.TrainerId;
      }
    }

    private void SetMoves(SavePokemonPayload payload)
    {
      Moves.Clear();
      if (payload.Moves?.Any() == true)
      {
        Moves.AddRange(payload.Moves.Select(move => new PokemonMove(move)));
      }
    }

    public override string ToString() => $"{Surname ?? SpeciesName} | {base.ToString()}";
  }
}
