using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonCreated : DomainEvent
  {
    public PokemonCreated(
      Dictionary<Statistic, byte> baseStatistics,
      LevelingRate levelingRate,
      CreatePokemonPayload payload,
      string speciesName
    )
    {
      BaseStatistics = baseStatistics ?? throw new ArgumentNullException(nameof(baseStatistics));
      LevelingRate = levelingRate;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
      SpeciesName = speciesName ?? throw new ArgumentNullException(nameof(speciesName));
    }

    public Dictionary<Statistic, byte> BaseStatistics { get; private set; }
    public LevelingRate LevelingRate { get; private set; }
    public CreatePokemonPayload Payload { get; private set; }
    public string SpeciesName { get; private set; }
  }
}
