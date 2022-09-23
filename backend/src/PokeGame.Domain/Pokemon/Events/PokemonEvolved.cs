using MediatR;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonEvolved : DomainEvent, INotification
  {
    public PokemonEvolved(
      Dictionary<Statistic, byte> baseStatistics,
      double? genderRatio,
      LevelingRate levelingRate,
      EvolvePokemonPayload payload,
      bool removeHeldItem,
      string speciesName
    )
    {
      BaseStatistics = baseStatistics;
      GenderRatio = genderRatio;
      LevelingRate = levelingRate;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
      RemoveHeldItem = removeHeldItem;
      SpeciesName = speciesName;
    }

    public Dictionary<Statistic, byte> BaseStatistics { get; private set; }
    public double? GenderRatio { get; private set; }
    public LevelingRate LevelingRate { get; private set; }
    public EvolvePokemonPayload Payload { get; private set; }
    public bool RemoveHeldItem { get; private set; }
    public string SpeciesName { get; private set; }

    public static PokemonEvolved Create(EvolvePokemonPayload payload, Species.Species species, bool removeHeldItem)
    {
      ArgumentNullException.ThrowIfNull(payload);
      ArgumentNullException.ThrowIfNull(species);

      return new(species.BaseStatistics, species.GenderRatio, species.LevelingRate, payload, removeHeldItem, species.Name);
    }
  }
}
