using MediatR;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonCreated : DomainEvent, INotification
  {
    public PokemonCreated(
      byte baseFriendship,
      Dictionary<Statistic, byte> baseStatistics,
      double? genderRatio,
      LevelingRate levelingRate,
      CreatePokemonPayload payload,
      string speciesName
    )
    {
      BaseFriendship = baseFriendship;
      BaseStatistics = baseStatistics;
      GenderRatio = genderRatio;
      LevelingRate = levelingRate;
      Payload = payload;
      SpeciesName = speciesName;
    }

    public byte BaseFriendship { get; private set; }
    public Dictionary<Statistic, byte> BaseStatistics { get; private set; }
    public double? GenderRatio { get; private set; }
    public LevelingRate LevelingRate { get; private set; }
    public CreatePokemonPayload Payload { get; private set; }
    public string SpeciesName { get; private set; }

    public static PokemonCreated Create(CreatePokemonPayload payload, Species.Species species)
    {
      ArgumentNullException.ThrowIfNull(payload);
      ArgumentNullException.ThrowIfNull(species);

      return new PokemonCreated(
        species.BaseFriendship,
        species.BaseStatistics,
        species.GenderRatio,
        species.LevelingRate,
        payload,
        species.Name
      );
    }
  }
}
