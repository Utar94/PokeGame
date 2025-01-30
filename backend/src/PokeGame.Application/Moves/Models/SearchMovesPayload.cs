using Logitar.Portal.Contracts.Search;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Models;

public record SearchMovesPayload : SearchPayload
{
  public PokemonType? Type { get; set; }
  public MoveCategory? Category { get; set; }

  public new List<MoveSortOption> Sort { get; set; } = [];
}
