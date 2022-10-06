using PokeGame.Domain.Pokemon;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class PokemonPositionEntity
  {
    public PokemonPositionEntity(PokemonEntity pokemon, TrainerEntity trainer)
    {
      Pokemon = pokemon;
      PokemonId = pokemon.Sid;
      Trainer = trainer;
      TrainerId = trainer.Sid;
    }
    private PokemonPositionEntity()
    {
    }

    public PokemonEntity? Pokemon { get; private set; }
    public int PokemonId { get; private set; }

    public TrainerEntity? Trainer { get; private set; }
    public int TrainerId { get; private set; }

    public byte Position { get; private set; }
    public byte Box { get; private set; }

    public void Synchronize(PokemonPosition pokemonPosition)
    {
      Position = pokemonPosition.Position;
      Box = pokemonPosition.Box ?? 0;
    }
  }
}
