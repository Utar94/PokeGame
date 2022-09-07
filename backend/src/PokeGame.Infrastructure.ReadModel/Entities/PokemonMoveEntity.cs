namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class PokemonMoveEntity
  {
    public PokemonEntity? Pokemon { get; set; }
    public int PokemonId { get; set; }

    public MoveEntity? Move { get; set; }
    public int MoveId { get; set; }

    public byte Position { get; set; }
    public byte RemainingPowerPoints { get; set; }
  }
}
