using PokeGame.Domain.Pokemon;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class PokemonMoveEntity
  {
    public PokemonMoveEntity(PokemonEntity pokemon, MoveEntity move)
    {
      Pokemon = pokemon;
      PokemonId = pokemon.Sid;
      Move = move;
      MoveId = move.Sid;
    }
    private PokemonMoveEntity()
    {
    }

    public PokemonEntity? Pokemon { get; private set; }
    public int PokemonId { get; private set; }

    public MoveEntity? Move { get; private set; }
    public int MoveId { get; private set; }

    public byte Position { get; private set; }
    public byte RemainingPowerPoints { get; private set; }

    public void Synchronize(PokemonMove pokemonMove)
    {
      Position = pokemonMove.Position;
      RemainingPowerPoints = pokemonMove.RemainingPowerPoints;
    }
  }
}
