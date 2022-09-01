using AutoMapper;
using PokeGame.Core.Models;
using PokeGame.Core.Species.Models;

namespace PokeGame.Core.Species
{
  internal class SpeciesProfile : Profile
  {
    public SpeciesProfile()
    {
      CreateMap<Species, SpeciesModel>()
        .IncludeBase<Aggregate, AggregateModel>()
        .ForMember(x => x.BaseStatistics, x => x.MapFrom(y => y.BaseStatistics.Select(pair => new StatisticValueModel
        {
          Statistic = pair.Key,
          Value = pair.Value
        })))
        .ForMember(x => x.EvYield, x => x.MapFrom(y => y.EvYield.Select(pair => new StatisticValueModel
        {
          Statistic = pair.Key,
          Value = pair.Value
        })));
      CreateMap<SpeciesModel, SpeciesSummary>()
        .IncludeBase<AggregateModel, AggregateSummary>();
    }
  }
}
