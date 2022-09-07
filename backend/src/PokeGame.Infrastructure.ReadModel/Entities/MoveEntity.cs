using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class MoveEntity : Entity
  {
    public PokemonType Type { get; set; }
    public MoveCategory Category { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public byte? Accuracy { get; set; }
    public byte? Power { get; set; }
    public byte PowerPoints { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }

    public List<PokemonMoveEntity> PokemonMoves { get; set; } = new();

    public void Synchronize(Move move)
    {
      base.Synchronize(move);

      Type = move.Type;
      Category = move.Category;

      Name = move.Name;
      Description = move.Description;

      Accuracy = move.Accuracy;
      Power = move.Power;
      PowerPoints = move.PowerPoints;

      Notes = move.Notes;
      Reference = move.Reference;
    }
  }
}
