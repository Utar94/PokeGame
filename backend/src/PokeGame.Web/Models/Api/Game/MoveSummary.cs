using PokeGame.Application.Moves.Models;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Web.Models.Api.Game
{
  public class PokemonMoveSummary
  {
    public PokemonMoveSummary(PokemonMoveModel pokemonMove)
    {
      MoveModel move = pokemonMove.Move ?? throw new ArgumentException($"The {nameof(pokemonMove.Move)} is required.", nameof(pokemonMove));

      Position = pokemonMove.Position;

      Type = move.Type;
      Category = move.Category;

      Accuracy = move.Accuracy;
      Power = move.Power;
      PowerPoints = move.PowerPoints;
      RemainingPowerPoints = pokemonMove.RemainingPowerPoints;

      Name = move.Name;
      Description = move.Description;
    }

    public byte Position { get; set; }

    public PokemonType Type { get; set; }
    public MoveCategory Category { get; set; }

    public byte? Accuracy { get; set; }
    public byte? Power { get; set; }
    public byte PowerPoints { get; set; }
    public byte RemainingPowerPoints { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }
  }
}
