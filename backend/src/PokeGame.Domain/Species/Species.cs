using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Species.Events;
using PokeGame.Domain.Species.Payloads;
using System.IO.Pipes;

namespace PokeGame.Domain.Species
{
  public class Species : Aggregate
  {
    public Species(CreateSpeciesPayload payload)
    {
      ApplyChange(new SpeciesCreated(payload));
    }
    private Species()
    {
    }

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

    public Dictionary<Statistic, byte> BaseStatistics { get; private set; } = new();
    public Dictionary<Statistic, byte> EvYield { get; private set; } = new();

    public string? Notes { get; private set; }
    public string? Picture { get; private set; }
    public string? Reference { get; private set; }

    public List<Guid> AbilityIds { get; private set; } = new();
    public List<Evolution> Evolutions { get; private set; } = new();

    public void Delete() => ApplyChange(new SpeciesDeleted());
    public void Update(UpdateSpeciesPayload payload) => ApplyChange(new SpeciesUpdated(payload));

    protected virtual void Apply(SpeciesCreated @event)
    {
      Number = @event.Payload.Number;

      PrimaryType = @event.Payload.PrimaryType;
      SecondaryType = @event.Payload.SecondaryType;

      Apply(@event.Payload);
    }
    protected virtual void Apply(SpeciesDeleted @event)
    {
      Delete(@event);
    }
    protected virtual void Apply(SpeciesUpdated @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SaveSpeciesPayload payload)
    {
      Name = payload.Name.Trim();
      Category = payload.Category?.CleanTrim();
      Description = payload.Description?.CleanTrim();

      GenderRatio = payload.GenderRatio;
      Height = payload.Height;
      Weight = payload.Weight;

      BaseExperienceYield = payload.BaseExperienceYield;
      BaseFriendship = payload.BaseFriendship;
      CatchRate = payload.CatchRate;
      LevelingRate = payload.LevelingRate;

      BaseStatistics.Clear();
      if (payload.BaseStatistics != null)
      {
        foreach (StatisticValuePayload pair in payload.BaseStatistics)
        {
          BaseStatistics[pair.Statistic] = pair.Value;
        }
      }

      EvYield.Clear();
      if (payload.EvYield != null)
      {
        foreach (StatisticValuePayload pair in payload.EvYield)
        {
          EvYield[pair.Statistic] = pair.Value;
        }
      }

      Notes = payload.Notes?.CleanTrim();
      Picture = payload.Picture;
      Reference = payload.Reference;

      AbilityIds.Clear();
      if (payload.AbilityIds != null)
      {
        AbilityIds.AddRange(payload.AbilityIds);
      }

      Evolutions.Clear();
      if (payload.Evolutions != null)
      {
        foreach (EvolutionPayload evolution in payload.Evolutions)
        {
          switch (evolution.Method)
          {
            case EvolutionMethod.Item:
              Evolutions.Add(Evolution.Item(evolution.SpeciesId, evolution.ItemId ?? default, evolution.Gender, evolution.Region, evolution.Notes));
              break;
            case EvolutionMethod.LevelUp:
              Evolutions.Add(Evolution.LevelUp(evolution.SpeciesId, evolution.Gender, evolution.HighFriendship, evolution.ItemId,
                evolution.Level, evolution.Location, evolution.MoveId, evolution.Region, evolution.TimeOfDay, evolution.Notes));
              break;
            case EvolutionMethod.Trade:
              Evolutions.Add(Evolution.Trade(evolution.SpeciesId, evolution.ItemId, evolution.Notes));
              break;
            default:
              throw new ArgumentException($"The evolution method '{evolution.Method}' is not supported.", nameof(payload));
          }
        }
      }
    }

    public override string ToString() => $"No. {Number:d3} {Name} | {base.ToString()}";
  }
}
