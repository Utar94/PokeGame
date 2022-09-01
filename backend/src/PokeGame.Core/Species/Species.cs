using PokeGame.Core.Pokemon;
using PokeGame.Core.Species.Events;
using PokeGame.Core.Species.Payloads;

namespace PokeGame.Core.Species
{
  public class Species : Aggregate
  {
    public Species(CreateSpeciesPayload payload, Guid userId)
    {
      ApplyChange(new CreatedEvent(payload, userId));
    }
    private Species()
    {
    }

    public int Number { get; private set; }

    public PokemonType PrimaryType { get; private set; }
    public PokemonType? SecondaryType { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Category { get; private set; }
    public string? Description { get; private set; }

    public double? GenderRatio { get; private set; }
    public double? Height { get; private set; }
    public double? Weight { get; private set; }

    public int? BaseExperienceYield { get; private set; }
    public byte BaseFriendship { get; private set; }
    public byte? CatchRate { get; private set; }
    public LevelingRate LevelingRate { get; private set; }

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public Dictionary<Statistic, byte> BaseStatistics { get; private set; } = new();
    public string? BaseStatisticsSerialized
    {
      get => FormatDictionary(BaseStatistics);
      private set => FillDictionary(BaseStatistics, value);
    }
    public Dictionary<Statistic, byte> EvYield { get; private set; } = new();
    public string? EvYieldSerialized
    {
      get => FormatDictionary(EvYield);
      private set => FillDictionary(EvYield, value);
    }

    public List<Abilities.Ability> Abilities { get; private set; } = new();

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateSpeciesPayload payload, Guid userId)
    {
      ApplyChange(new UpdatedEvent(payload, userId));
    }

    protected virtual void Apply(CreatedEvent @event)
    {
      Number = @event.Payload.Number;

      PrimaryType = @event.Payload.PrimaryType;
      SecondaryType = @event.Payload.SecondaryType;

      Apply(@event.Payload);
    }
    protected virtual void Apply(DeletedEvent @event)
    {
    }
    protected virtual void Apply(UpdatedEvent @event)
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

      Notes = payload.Notes?.CleanTrim();
      Reference = payload.Reference;
    }

    public override string ToString() => $"No. {Number:d3} {Name} | {base.ToString()}";

    private static string? FormatDictionary<T>(IDictionary<T, byte> dictionary)
    {
      return string.Join('|', dictionary.Where(x => x.Value != 0).Select(pair => $"{pair.Key}:{pair.Value}"))
        .CleanTrim();
    }
    private static void FillDictionary<T>(IDictionary<T, byte> dictionary, string? s) where T : struct
    {
      dictionary.Clear();

      if (s != null)
      {
        string[] values = s.Split('|');
        foreach (string value in values)
        {
          string[] pair = value.Split(':');
          dictionary[Enum.Parse<T>(pair[0])] = byte.Parse(pair[1]);
        }
      }
    }
  }
}
