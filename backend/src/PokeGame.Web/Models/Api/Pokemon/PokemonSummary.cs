using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon;
using PokeGame.Web.Models.Api.Ability;
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
      if (model.Ability == null)
      {
        throw new ArgumentException($"The {nameof(model.Ability)} is required.", nameof(model));
      }

      Species = new SpeciesSummary(model.Species);
      Ability = new AbilitySummary(model.Ability);

      Level = model.Level;

      Gender = model.Gender;
      Surname = model.Surname;

      Attack = model.Statistics.SingleOrDefault(x => x.Statistic == Statistic.Attack)?.Value ?? 0;
      SpecialAttack = model.Statistics.SingleOrDefault(x => x.Statistic == Statistic.SpecialAttack)?.Value ?? 0;
      Speed = model.Statistics.SingleOrDefault(x => x.Statistic == Statistic.Speed)?.Value ?? 0;

      CurrentHitPoints = model.CurrentHitPoints;
      MaximumHitPoints = model.Statistics.SingleOrDefault(x => x.Statistic == Statistic.HP)?.Value ?? 0;
      StatusCondition = model.StatusCondition;

      Moves = model.Moves;
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
    public AbilitySummary Ability { get; set; } = null!;

    public byte Level { get; set; }

    public PokemonGender Gender { get; set; }
    public string? Surname { get; set; }

    public short Attack { get; set; }
    public short SpecialAttack { get; set; }
    public short Speed { get; set; }

    public short CurrentHitPoints { get; set; }
    public short MaximumHitPoints { get; set; }
    public StatusCondition? StatusCondition { get; set; }

    public IEnumerable<PokemonMoveModel> Moves { get; set; } = Enumerable.Empty<PokemonMoveModel>();
    public ItemSummary? HeldItem { get; set; }

    public TrainerSummary? Trainer { get; set; }
    public byte? Position { get; set; }
    public byte? Box { get; set; }
  }
}
