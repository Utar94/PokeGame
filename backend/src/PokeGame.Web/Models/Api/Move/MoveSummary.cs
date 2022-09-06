using PokeGame.Application.Moves.Models;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Web.Models.Api.Move
{
  public class MoveSummary : AggregateSummary
  {
    public MoveSummary(MoveModel model) : base(model)
    {
      Type = model.Type;
      Category = model.Category;
      Name = model.Name;
    }

    public PokemonType Type { get; set; }
    public MoveCategory Category { get; set; }

    public string Name { get; set; } = string.Empty;
  }
}
