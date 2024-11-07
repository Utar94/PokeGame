using Logitar.Portal.Contracts.Search;

namespace PokeGame.Contracts.Moves;

public record SearchMovesPayload : SearchPayload
{
  public PokemonType? Type { get; set; }
  public MoveCategory? Category { get; set; }

  public new List<MoveSortOption> Sort { get; set; } = [];
}
