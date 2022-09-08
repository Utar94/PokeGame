using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon;
using PokeGame.Web.Models.Api.Items;
using PokeGame.Web.Models.Api.Species;
using PokeGame.Web.Models.Api.Trainer;

namespace PokeGame.Web.Models.Api.Pokemon
{
  public class PokemonSummary : AggregateSummary
  {
    public PokemonSummary(PokemonModel model) : base(model)
    {
      if (model.Species == null)
      {
        throw new ArgumentException($"The {nameof(model.Species)} is required.", nameof(model));
      }

      Species = new SpeciesSummary(model.Species);

      Level = model.Level;

      Gender = model.Gender;
      Surname = model.Surname;

      if (model.HeldItem != null)
      {
        HeldItem = new ItemSummary(model.HeldItem);
      }

      if (model.History?.Trainer != null)
      {
        Trainer = new TrainerSummary(model.History.Trainer);
      }
      Position = model.Position;
      Box = model.Box;
    }

    public SpeciesSummary Species { get; set; } = null!;

    public byte Level { get; set; }

    public PokemonGender Gender { get; set; }
    public string? Surname { get; set; }

    public ItemSummary? HeldItem { get; set; }

    public TrainerSummary? Trainer { get; set; }
    public byte? Position { get; set; }
    public byte? Box { get; set; }
  }
}
