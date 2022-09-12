namespace PokeGame.Domain.Pokemon
{
  public class PokemonPosition
  {
    public PokemonPosition(byte position, byte? box = null)
    {
      Position = position;
      Box = box;
    }

    public byte Position { get; }
    public byte? Box { get; }
  }
}
