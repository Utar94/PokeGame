using AutoMapper;
using PokeGame.Application.Models;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Application.Species.Models;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain.Pokemon;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Profiles
{
  internal class PokemonProfile : Profile
  {
    public PokemonProfile()
    {
      CreateMap<PokemonEntity, PokemonModel>()
        .IncludeBase<Entity, AggregateModel>()
        .ForMember(x => x.Box, x => x.MapFrom(y => (y.Position == null || y.Position.Box == 0) ? (byte?)null : y.Position.Box))
        .ForMember(x => x.EffortValues, x => x.MapFrom(y => ParseStatisticValues(y.EffortValues)))
        .ForMember(x => x.ExperienceThreshold, x => x.MapFrom(GetExperienceThreshold))
        .ForMember(x => x.History, x => x.MapFrom(GetHistory))
        .ForMember(x => x.IndividualValues, x => x.MapFrom(y => ParseStatisticValues(y.IndividualValues)))
        .ForMember(x => x.MaximumHitPoints, x => x.MapFrom(y => GetStatistic(y, Statistic.HP)))
        .ForMember(x => x.Position, x => x.MapFrom(y => y.Position == null ? (byte?)null : y.Position.Position))
        .ForMember(x => x.Attack, x => x.MapFrom(y => GetStatistic(y, Statistic.Attack)))
        .ForMember(x => x.Defense, x => x.MapFrom(y => GetStatistic(y, Statistic.Defense)))
        .ForMember(x => x.SpecialAttack, x => x.MapFrom(y => GetStatistic(y, Statistic.SpecialAttack)))
        .ForMember(x => x.SpecialDefense, x => x.MapFrom(y => GetStatistic(y, Statistic.SpecialDefense)))
        .ForMember(x => x.Speed, x => x.MapFrom(y => GetStatistic(y, Statistic.Speed)));
      CreateMap<PokemonMoveEntity, PokemonMoveModel>();
    }

    private static ushort GetStatistic(PokemonEntity pokemon, Statistic statistic)
    {
      string[] pairs = pokemon.Statistics?.Split('|') ?? Array.Empty<string>();

      foreach (string pair in pairs)
      {
        string[] values = pair.Split(':');
        if (values[0] == statistic.ToString())
        {
          return ushort.Parse(values[1]);
        }
      }

      return 0;
    }

    private static uint? GetExperienceThreshold(PokemonEntity pokemon, PokemonModel model)
    {
      if (pokemon.Species == null)
      {
        return null;
      }

      uint threshold = ExperienceTable.GetTotalExperience(pokemon.Species.LevelingRate, (byte)(pokemon.Level + 1));

      return threshold == 0 ? null : threshold;
    }

    private static HistoryModel? GetHistory(PokemonEntity pokemon, PokemonModel model, HistoryModel? member, ResolutionContext context)
    {
      if (pokemon.MetAtLevel.HasValue && pokemon.MetLocation != null
        && pokemon.MetOn.HasValue && pokemon.CurrentTrainer != null)
      {
        return new HistoryModel
        {
          Level = pokemon.MetAtLevel.Value,
          Location = pokemon.MetLocation,
          MetOn = pokemon.MetOn.Value,
          Trainer = pokemon.CurrentTrainer == null ? null : context.Mapper.Map<TrainerModel>(pokemon.CurrentTrainer)
        };
      }

      return null;
    }

    private static IEnumerable<StatisticValueModel> ParseStatisticValues(string? value)
    {
      string[] pairs = value?.Split('|') ?? Array.Empty<string>();

      return pairs.Select(pair =>
      {
        string[] values = pair.Split(':');

        return new StatisticValueModel
        {
          Statistic = Enum.Parse<Statistic>(values[0]),
          Value = byte.Parse(values[1])
        };
      });
    }
  }
}
