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
        .ForMember(x => x.EffortValues, x => x.MapFrom(GetEffortValues))
        .ForMember(x => x.History, x => x.MapFrom(GetHistory))
        .ForMember(x => x.IndividualValues, x => x.MapFrom(GetIndividualValues))
        .ForMember(x => x.Statistics, x => x.MapFrom(GetStatistics));
      CreateMap<PokemonMoveEntity, PokemonMoveModel>();
    }

    private static IEnumerable<StatisticValueModel> GetEffortValues(PokemonEntity pokemon, PokemonModel model)
    {
      return ParseStatisticValues(pokemon.EffortValues);
    }
    private static IEnumerable<StatisticValueModel> GetIndividualValues(PokemonEntity pokemon, PokemonModel model)
    {
      return ParseStatisticValues(pokemon.IndividualValues);
    }
    private static IEnumerable<PokemonStatisticModel> GetStatistics(PokemonEntity pokemon, PokemonModel model)
    {
      string[] pairs = pokemon.Statistics?.Split('|') ?? Array.Empty<string>();

      return pairs.Select(pair =>
      {
        string[] values = pair.Split(':');

        return new PokemonStatisticModel
        {
          Statistic = Enum.Parse<Statistic>(values[0]),
          Value = short.Parse(values[1])
        };
      });
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
  }
}
