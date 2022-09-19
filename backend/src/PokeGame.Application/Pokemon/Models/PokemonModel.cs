using PokeGame.Application.Abilities.Models;
using PokeGame.Application.Items.Models;
using PokeGame.Application.Models;
using PokeGame.Application.Species.Models;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Application.Pokemon.Models
{
  public class PokemonModel : AggregateModel
  {
    public SpeciesModel? Species { get; set; }
    public AbilityModel? Ability { get; set; }

    public byte Level { get; set; }
    public int Experience { get; set; }

    public PokemonGender Gender { get; set; }
    public string Nature { get; set; } = null!;
    public string? Surname { get; set; }
    public string? Description { get; set; }

    public IEnumerable<StatisticValueModel> IndividualValues { get; set; } = Enumerable.Empty<StatisticValueModel>();
    public IEnumerable<StatisticValueModel> EffortValues { get; set; } = Enumerable.Empty<StatisticValueModel>();
    public short MaximumHitPoints { get; set; }
    public short Attack { get; set; }
    public short Defense { get; set; }
    public short SpecialAttack { get; set; }
    public short SpecialDefense { get; set; }
    public short Speed { get; set; }

    public short CurrentHitPoints { get; set; }
    public StatusCondition? StatusCondition { get; set; }

    public IEnumerable<PokemonMoveModel> Moves { get; set; } = Enumerable.Empty<PokemonMoveModel>();
    public ItemModel? HeldItem { get; set; }

    public HistoryModel? History { get; set; }
    public TrainerModel? OriginalTrainer { get; set; }
    public byte? Position { get; set; }
    public byte? Box { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
