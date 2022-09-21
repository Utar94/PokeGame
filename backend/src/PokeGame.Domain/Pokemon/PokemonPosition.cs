namespace PokeGame.Domain.Pokemon
{
  public class PokemonPosition
  {
    public PokemonPosition(byte position, byte? box = null)
    {
      Position = position;
      Box = box;
    }

    public byte Position { get; private set; }
    public byte? Box { get; private set; }

    public override bool Equals(object? obj) => obj is PokemonPosition position
      && position.Position == Position
      && position.Box == Box;
    public override int GetHashCode() => HashCode.Combine(typeof(PokemonPosition), Position, Box);
    public override string ToString() => $"({(Box.HasValue ? $"Box={Box.Value}" : "Party")}, Position={Position})";
  }
}
