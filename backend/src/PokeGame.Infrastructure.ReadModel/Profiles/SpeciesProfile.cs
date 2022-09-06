using AutoMapper;
using PokeGame.Application.Models;
using PokeGame.Application.Species.Models;
using PokeGame.Domain.Pokemon;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Profiles
{
  internal class SpeciesProfile : Profile
  {
    public SpeciesProfile()
    {
      CreateMap<Species, SpeciesModel>()
        .IncludeBase<Entity, AggregateModel>()
        .ForMember(x => x.BaseStatistics, x => x.MapFrom(GetBaseStatistics))
        .ForMember(x => x.EvYield, x => x.MapFrom(GetEvYield));
    }

    private static IEnumerable<StatisticValueModel> GetBaseStatistics(Species species, SpeciesModel model)
    {
      return ParseStatisticValues(species.BaseStatistics);
    }
    private static IEnumerable<StatisticValueModel> GetEvYield(Species species, SpeciesModel model)
    {
      return ParseStatisticValues(species.EvYield);
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
