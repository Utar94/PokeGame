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
    public int? ExperienceThreshold { get; set; }
    public int? ExperienceToNextLevel => ExperienceThreshold.HasValue ? (ExperienceThreshold.Value - Experience) : null;
    public byte Friendship { get; set; }

    public PokemonGender Gender { get; set; }
    public string Nature { get; set; } = null!;
    public string? Surname { get; set; }
    public string? Description { get; set; }

    public IEnumerable<StatisticValueModel> IndividualValues { get; set; } = Enumerable.Empty<StatisticValueModel>();
    public IEnumerable<StatisticValueModel> EffortValues { get; set; } = Enumerable.Empty<StatisticValueModel>();
    public ushort MaximumHitPoints { get; set; }
    public ushort Attack { get; set; }
    public ushort Defense { get; set; }
    public ushort SpecialAttack { get; set; }
    public ushort SpecialDefense { get; set; }
    public ushort Speed { get; set; }

    public ushort CurrentHitPoints { get; set; }
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
