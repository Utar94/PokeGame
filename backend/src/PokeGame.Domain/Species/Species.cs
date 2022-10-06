using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Species.Events;
using PokeGame.Domain.Species.Payloads;

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
    public Dictionary<Guid, Evolution> Evolutions { get; private set; } = new();
    public Dictionary<Region, int> RegionalNumbers { get; private set; } = new();

    public void Delete() => ApplyChange(new SpeciesDeleted());
    public void Update(UpdateSpeciesPayload payload) => ApplyChange(new SpeciesUpdated(payload));

    public void RemoveEvolution(Species evolvedSpecies)
    {
      if (!Evolutions.ContainsKey(evolvedSpecies.Id))
      {
        throw new SpeciesEvolutionNotFoundException(Id, evolvedSpecies.Id);
      }

      ApplyChange(new SpeciesEvolutionRemoved(evolvedSpecies.Id));
    }
    public void SaveEvolution(Species evolvedSpecies, SaveEvolutionPayload payload) => ApplyChange(new SpeciesEvolutionSaved(evolvedSpecies.Id, payload));

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
    protected virtual void Apply(SpeciesEvolutionRemoved @event)
    {
      Evolutions.Remove(@event.SpeciesId);
    }
    protected virtual void Apply(SpeciesEvolutionSaved @event)
    {
      SaveEvolutionPayload payload = @event.Payload;

      Evolutions[@event.SpeciesId] = payload.Method switch
      {
        EvolutionMethod.Item => Evolution.Item(payload.ItemId ?? default, payload.Gender, payload.Region, payload.Notes),
        EvolutionMethod.LevelUp => Evolution.LevelUp(payload.Gender, payload.HighFriendship, payload.ItemId,
          payload.Level, payload.Location, payload.MoveId, payload.Region, payload.TimeOfDay, payload.Notes),
        EvolutionMethod.Trade => Evolution.Trade(payload.ItemId, payload.Notes),
        _ => throw new ArgumentException($"The evolution method '{payload.Method}' is not supported.", nameof(@event)),
      };
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

      RegionalNumbers.Clear();
      if (payload.RegionalNumbers != null)
      {
        foreach (RegionalNumberPayload regionalNumber in payload.RegionalNumbers)
        {
          RegionalNumbers[regionalNumber.Region] = regionalNumber.Number;
        }
      }
    }

    public override string ToString() => $"No. {Number:d3} {Name} | {base.ToString()}";
  }
}
